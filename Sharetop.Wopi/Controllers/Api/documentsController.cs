using Aspose.Words;
using Newtonsoft.Json;
using Sharetop.Wopi.Models;
using Sharetop.Wopi.Models.Api;
using Sharetop.Wopi.Security;
using Sharetop.Wopi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Sharetop.Wopi.Controllers.Api
{
    [ApiProofValidationFilter]
    public class documentsController : ApiController
    {
        [ApiProofValidationFilter]
        [HttpGet]
        [Route("api/documents/{id}")]
        public HttpResponseMessage Get(Guid id)
        {
            HttpResponseMessage response;

            try
            {
                var f = File.Get(id);

                FileModel fm = JsonConvert.DeserializeObject<FileModel>(f.ToString());

                var jsonString = JsonConvert.SerializeObject(fm,Formatting.Indented, (new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


                response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
                response.Content = new StringContent(jsonString);
                
            }
            catch (Exception ex)
            {
                ServerUtil.LogException(ex);
                response = ServerUtil.returnStatus(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }

        [ApiProofValidationFilter]
        [HttpGet]
        [Route("api/documents/{id}/url")]
        public async Task<HttpResponseMessage> GetUrl(Guid id,[FromUri]UserAction ua)
        {
            try
            {
                var f = File.Get(id);
                if (String.IsNullOrEmpty(ua.Action))
                {
                    ua.Action = "view";
                }

                // Write the response and return success 200
                var response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
                response.Content = new StringContent(await f.Url(ua));
                return response;
            }
            catch (Exception ex)
            {
                ServerUtil.LogException(ex);
                return ServerUtil.returnStatus(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ApiProofValidationFilter]
        [HttpGet]
        [Route("api/documents/{id}/actions")]
        public async Task<HttpResponseMessage> GetActions(Guid id)
        {
            try
            {
                var f = File.Get(id);

                if (f.Actions == null)
                {
                    await f.PopulateActions();
                    f.Save();
                }

                string[] actions;

                if (f.Actions != null)
                {
                    actions = new string[f.Actions.Count];

                    for (int i = 0; i < f.Actions.Count; i++)
                    {
                        actions[i] = f.Actions[i].name;
                    }
                }
                else
                {
                    actions = new string[0];
                }


                // Write the response and return success 200
                var response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
                response.Content = new StringContent(JsonConvert.SerializeObject(actions));
                return response;
            }
            catch (Exception ex)
            {
                ServerUtil.LogException(ex);
                return ServerUtil.returnStatus(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ApiProofValidationFilter]
        [HttpGet]
        [Route("api/documents/{id}/revisions")]
        public HttpResponseMessage GetRevisions(Guid id)
        {
            try
            {
                var f = File.Get(id);

                // Write the response and return success 200
                var response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
                response.Content = new StringContent(JsonConvert.SerializeObject(f.Revisions));
                return response;
            }
            catch (Exception ex)
            {
                ServerUtil.LogException(ex);
                return ServerUtil.returnStatus(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ApiProofValidationFilter]
        [HttpPost]
        [Route("api/documents/{id}/makerevision")]
        public HttpResponseMessage MakeRevision(Guid id,[FromBody]FileRevision revision)
        {
            try
            {
                var f = File.Get(id);

                if (String.IsNullOrEmpty(revision.UserId))
                {
                    revision.UserId = "Anonymous";
                }

                var d = f.MakeRevision(revision.UserId);

                // Write the response and return success 200
                var response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
                response.Content = new StringContent(d.ToString("yyyyMMddHHmmssfff"));
                return response;
            }
            catch (Exception ex)
            {
                ServerUtil.LogException(ex);
                return ServerUtil.returnStatus(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ApiProofValidationFilter]
        [HttpGet]
        [Route("api/documents/{id}/contents")]
        public HttpResponseMessage Contents(Guid id)
        {
            try
            {
                var f = File.Get(id);
                DateTime? d;
                try
                {
                    d = DateTime.Parse(Request.GetQueryNameValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase)["timepoint"]);
                }
                catch
                {
                    d = null;
                }

                // Write the response and return success 200
                var response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
                response.Content = new ByteArrayContent(f.ReadContent(d));
                return response;
            }
            catch (Exception ex)
            {
                ServerUtil.LogException(ex);
                return ServerUtil.returnStatus(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ApiProofValidationFilter]
        [HttpPost]
        [Route("api/documents/{id}")]
        public async Task<HttpResponseMessage> Post(String id,[FromBody]FileInfo info)
        {
            HttpResponseMessage response;
            File f;

            switch (id.ToLower())
            {
                case "create":

                    f = File.Create();

                    f.BaseFileName = info.Caption + "." + info.Extension;
                    f.OwnerId = info.OwnerId;

                    await f.PopulateActions();
                
                    if (!String.IsNullOrEmpty(info.Template))
                    {
                        //提供了模板
                        try
                        {
                            f.WriteContent(buildFromTemplate(info));
                        }
                        catch(Exception ex)
                        {
                            ServerUtil.LogException(ex);
                        }
                    }

                    f.Save();

                    response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
                    response.Content = new StringContent(f.id.ToString());

                    break;
                default:
                    try
                    {
                        f = File.Get(Guid.Parse(id));

                        f.BaseFileName = info.Caption + "." + info.Extension;
                        f.UserId = info.OwnerId;

                        await f.PopulateActions();

                        if (!String.IsNullOrEmpty(info.Template))
                        {
                            //提供了模板
                            try
                            {
                                f.WriteContent(buildFromTemplate(info));
                            }
                            catch (Exception ex)
                            {
                                ServerUtil.LogException(ex);
                            }
                        }

                        f.Save();

                        response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
                        response.Content = new StringContent(f.id.ToString());
                    }
                    catch(Exception ex)
                    {
                        ServerUtil.LogException(ex);
                        response = ServerUtil.returnStatus(HttpStatusCode.InternalServerError, ex.Message);
                    }
                    break;
            }

            return response;
        }

        private byte[] buildFromTemplate(FileInfo info)
        {
            var tFile = ServerUtil.MapPath(String.Format("/App_Data/Template/{0}.{1}", info.Template, info.Extension));
            if (!System.IO.File.Exists(tFile))
            {
                throw new Exception("Template not found.");
            }

            if (String.IsNullOrEmpty(info.Caption)) info.Caption = "";
            if (String.IsNullOrEmpty(info.Content)) info.Content = "";
            if (String.IsNullOrEmpty(info.From)) info.From = "";
            if (String.IsNullOrEmpty(info.To)) info.To = "";
            if (String.IsNullOrEmpty(info.Date)) info.Date = DateTime.Now.ToString("yyyy年M月d日");

            var doc = new Document(tFile);

            doc.Range.Replace("%CAPTION%", info.Caption, false, false);
            doc.Range.Replace("%TO%", info.To, false, false);
            doc.Range.Replace("%CONTENT%", info.Content, false, false);
            doc.Range.Replace("%FROM%", info.From, false, false);
            doc.Range.Replace("%DATE%", info.Date, false, false);

            var outStream = new System.IO.MemoryStream();

            doc.Save(outStream, SaveFormat.Docx);

            return outStream.ToArray();
        }

        [ApiProofValidationFilter]
        [HttpPost]
        [Route("api/documents/{id}/contents")]
        public async Task<HttpResponseMessage> PostContentsAsync(Guid id)
        {
            HttpResponseMessage response;
            try
            {
                var f = File.Get(id);
                if (!f.IsLocked())
                {
                    f.Lock("api.file.writer");
                    // If the file is 0 bytes, this is document creation
                    if (HttpContext.Current.Request.InputStream.Length > 0)
                    {
                        // Update the file in blob storage
                        var bytes = new byte[HttpContext.Current.Request.InputStream.Length];
                        HttpContext.Current.Request.InputStream.Read(bytes, 0, bytes.Length);

                        if (f.Extension() == "doc")
                        {
                            //更新为docx
                            var instream = new System.IO.MemoryStream(bytes);
                            var outstream = new System.IO.MemoryStream();
                            var doc = new Document(instream);

                            doc.Save(outstream, SaveFormat.Docx);

                            bytes = outstream.ToArray();
                            f.BaseFileName += "x";

                            await f.PopulateActions();

                            instream.Close();
                            outstream.Close();
                        }

                        f.WriteContent(bytes);

                        // Return success 200
                        response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
                    }
                    else
                    {
                        response = ServerUtil.returnStatus(HttpStatusCode.BadRequest, "No file content");
                    }

                    f.UnLock();
                }
                else
                {
                    response = ServerUtil.returnStatus(HttpStatusCode.Conflict, "File is locked");
                }
            }
            catch (Exception ex)
            {
                ServerUtil.LogException(ex);
                response = ServerUtil.returnStatus(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

    }
}
