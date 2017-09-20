using Newtonsoft.Json;
using Sharetop.Wopi.Models.Wopi;
using Sharetop.Wopi.Security;
using Sharetop.Wopi.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Sharetop.Wopi.Models.Api
{
    public class FileInfo
    {
        public string OwnerId { get; set; }
        public string Caption { get; set; }
        public string Extension { get; set; }
    }

    public class UserAction
    {
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string Action { get; set; }

        public UserAction()
        {
            Action = "view";
            UserId = "anonymous";
            UserDisplayName = "匿名";
        }
    }

    public class FileRevision
    {
        public string UserId { get; set; }
        public DateTime RevisionDate { get; set; }
    }

    public class File : FileModel
    {
        [JsonProperty(PropertyName = "CreationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty(PropertyName = "RevisionDate")]
        public DateTime RevisionDate { get; set; }

        [JsonProperty(PropertyName = "UserInfo")]
        public string UserInfo { get; set; }

        [JsonProperty(PropertyName = "CloseUrl")]
        public string CloseUrl { get; set; }

        [JsonProperty(PropertyName = "HostEditUrl")]
        public string HostEditUrl { get; set; }

        [JsonProperty(PropertyName = "HostViewUrl")]
        public string HostViewUrl { get; set; }

        [JsonProperty(PropertyName = "LockValue")]
        public string LockValue { get; set; }

        [JsonProperty(PropertyName = "LockExpires")]
        public DateTime? LockExpires { get; set; }

        [JsonProperty(PropertyName = "SupportsCoauth")]
        public bool SupportsCoauth
        {
            get { return false; }
        }

        [JsonProperty(PropertyName = "SupportsExtendedLockLength")]
        public bool SupportsExtendedLockLength
        {
            get { return false; }
        }

        [JsonProperty(PropertyName = "SupportsFileCreation")]
        public bool SupportsFileCreation
        {
            get { return false; }
        }

        [JsonProperty(PropertyName = "SupportsFolders")]
        public bool SupportsFolders
        {
            get { return false; }
        }

        [JsonProperty(PropertyName = "SupportsGetLock")]
        public bool SupportsGetLock
        {
            get { return true; }
        }

        [JsonProperty(PropertyName = "SupportsLocks")]
        public bool SupportsLocks
        {
            get { return true; }
        }

        [JsonProperty(PropertyName = "SupportsRename")]
        public bool SupportsRename
        {
            get { return true; }
        }

        [JsonProperty(PropertyName = "SupportsScenarioLinks")]
        public bool SupportsScenarioLinks
        {
            get { return false; }
        }

        [JsonProperty(PropertyName = "SupportsSecureStore")]
        public bool SupportsSecureStore
        {
            get { return false; }
        }

        [JsonProperty(PropertyName = "SupportsUpdate")]
        public bool SupportsUpdate
        {
            get { return true; }
        }

        [JsonProperty(PropertyName = "SupportsUserInfo")]
        public bool SupportsUserInfo
        {
            get { return true; }
        }

        [JsonProperty(PropertyName = "LicensesCheckForEditIsEnabled")]
        public bool LicensesCheckForEditIsEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Permissions for documents
        /// </summary>
        [JsonProperty(PropertyName = "ReadOnly")]
        public bool ReadOnly
        {
            get; set;
        }

        [JsonProperty(PropertyName = "RestrictedWebViewOnly")]
        public bool RestrictedWebViewOnly
        {
            get; set;
        }

        [JsonProperty(PropertyName = "UserCanAttend")] //Broadcast only
        public bool UserCanAttend
        {
            get { return false; }
        }

        [JsonProperty(PropertyName = "UserCanNotWriteRelative")]
        public bool UserCanNotWriteRelative
        {
            get { return false; }
        }

        [JsonProperty(PropertyName = "UserCanPresent")] //Broadcast only
        public bool UserCanPresent
        {
            get { return false; }
        }

        [JsonProperty(PropertyName = "UserCanRename")]
        public bool UserCanRename
        {
            get { return true; }
        }

        [JsonProperty(PropertyName = "UserCanWrite")]
        public bool UserCanWrite
        {
            get { return true; }
        }

        [JsonProperty(PropertyName = "WebEditingDisabled")]
        public bool WebEditingDisabled
        {
            get { return ReadOnly; }
        }

        [JsonProperty(PropertyName = "Actions")]
        public List<WopiAction> Actions { get; set; }

        [JsonProperty(PropertyName = "Revisions")]
        public List<FileRevision> Revisions { get; set; }

        public static File Create()
        {
            var f = new File();

            f.id = Guid.NewGuid();
            f.CreationDate = DateTime.Now;

            var filePath = string.Format("{0}\\{1}", ServerUtil.StoragePath(), f.id.ToString());
            if (Directory.Exists(filePath)) Directory.Delete(filePath,true);

            Directory.CreateDirectory(filePath);

            f.Save();

            return f; 
        }

        public bool IsLocked()
        {
            return !(String.IsNullOrEmpty(LockValue) || (LockExpires != null && LockExpires < DateTime.UtcNow));
        }

        public void Lock(string value)
        {
            LockValue = value;
            LockExpires = DateTime.UtcNow.AddMinutes(60);
            Save();
        }

        public void UnLock()
        {
            LockValue = null;
            LockExpires = null;
            Save();
        }

        public async System.Threading.Tasks.Task<string> Url(UserAction ua = null)
        {
            if (ua == null)
            {
                ua = new UserAction() ;
            }

            // Use discovery to determine endpoint to leverage
            List<WopiAction> discoData = await WopiUtil.GetDiscoveryInfo();
            var fileExt = BaseFileName.Substring(BaseFileName.LastIndexOf('.') + 1).ToLower();
            var action = discoData.FirstOrDefault(i => i.name == ua.Action && i.ext == fileExt);

            if (action != null)
            {
                string urlsrc = WopiUtil.GetActionUrl(action, this, ServerUtil.Config().Domain);

                WopiSecurity wopiSecurity = new WopiSecurity();

                var token = wopiSecurity.GenerateToken( id.ToString(), ua.UserId, ua.UserDisplayName);

                if (action.name == "edit")
                {
                    token.ReadOnly = false;
                }
                else
                {
                    token.ReadOnly = true;
                }
                token.Save();

                return String.Format("{0}&access_token={1}&access_token_ttl={2}",urlsrc, token.Identity, token.ValidTo().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString());
            }
            else
            {
                // This will only hit if the extension isn't supported by WOPI
                throw new Exception("Bad action on this file.");
            }
        }

        public byte[] ReadContent(DateTime? timePoint = null)
        {
            if (timePoint == null || Revisions.Count == 0)
            {
                var filePath = string.Format("{0}\\{1}\\latest", ServerUtil.StoragePath(), id.ToString());
                try
                {
                    return System.IO.File.ReadAllBytes(filePath);
                }
                catch (Exception ex)
                {
                    ServerUtil.LogException(ex);
                    throw ex;
                }
            }
            else
            {
                int i = 0;
                while (Revisions[i].RevisionDate > timePoint)
                {
                    i += 1;
                }

                if (i>= Revisions.Count)
                {
                    throw new Exception("timePoint too early");
                }
                else
                {
                    var filePath = string.Format("{0}\\{1}\\{2}", ServerUtil.StoragePath(), id.ToString(), Revisions[i].RevisionDate.ToString("yyyyMMddHHmmssfff"));
                    try
                    {
                        return System.IO.File.ReadAllBytes(filePath);
                    }
                    catch (Exception ex)
                    {
                        ServerUtil.LogException(ex);
                        throw ex;
                    }
                }
            }
        }

        public void WriteContent(byte[] content)
        {
            var filePath = string.Format("{0}\\{1}\\latest", ServerUtil.StoragePath(), id.ToString());
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            System.IO.File.WriteAllBytes(filePath, content);

            Version = DateTime.Now.ToString("s");
            Size = content.LongLength;

            Save();
        }

        public DateTime MakeRevision(string user = "Anonymous")
        {
            var fileDate = DateTime.Now;
            var filePath = string.Format("{0}\\{1}\\latest", ServerUtil.StoragePath(), id.ToString());
            var fileRevisionsPath = string.Format("{0}\\{1}\\{2}", ServerUtil.StoragePath(), id.ToString(), fileDate.ToString("yyyyMMddHHmmssfff"));

            if (System.IO.File.Exists(fileRevisionsPath)) System.IO.File.Delete(fileRevisionsPath);

            System.IO.File.Copy(filePath, fileRevisionsPath);

            if (Revisions == null)
            {
                Revisions = new List<FileRevision>();
            }

            Revisions.Insert(0, (new FileRevision() { UserId = user, RevisionDate = fileDate }));
            RevisionDate = fileDate;
            Save();

            return fileDate;
        }

        public void Save()
        {
            if (DatabaseUtil<File>.GetItems("Files", i => i.documentId == this.documentId).Count<File>() > 0)
            {
                if (!String.IsNullOrEmpty(LockValue))
                {
                    ServerUtil.Log("Write Lock " + LockValue + " : " + LockExpires.ToString());
                }
                else
                {
                    ServerUtil.Log("Clear Lock ");
                }

                DatabaseUtil<File>.UpdateItem("Files", this);
            }
            else
            {
                DatabaseUtil<File>.CreateItem("Files", this);
            }
        }

        public static File Get(Guid ID)
        {
            return DatabaseUtil<File>.GetItem("Files", i => i.id == ID);
        }


        public static IEnumerable<File> Select(Expression<Func<File, bool>> predicate)
        {
            return DatabaseUtil<File>.GetItems("Files", predicate);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented, (new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}