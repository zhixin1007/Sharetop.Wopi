using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;
using Sharetop.Wopi.Models.Api;

namespace Sharetop.Wopi.Utils
{
    public class ConfigBag
    {
        protected ConfigBag()
        {
            WopiDiscovery = "http://owa.company.com/hosting/discovery";
            SecureKey = "abcd1234";

            DatabaseServer = "127.0.0.1";
            DatabaseName = "Sharetop";
            DatabaseUser = "sharetop";
            DatabasePass = "";

            Domain = "";

            Encrypt = false;

            if (System.IO.File.Exists(ServerUtil.MapPath("~/App_Data/Config.txt")) == false)
            {
                Save();
            }
        }

        [JsonProperty(PropertyName = "Encrypt")]
        public bool Encrypt { get; set; }

        [JsonProperty(PropertyName = "SecureKey")]
        public string SecureKey { get; set; }

        [JsonProperty(PropertyName = "WopiDiscovery")]
        public string WopiDiscovery { get; set; }

        [JsonProperty(PropertyName = "DatabaseServer")]
        public string DatabaseServer { get; set; }

        [JsonProperty(PropertyName = "DatabaseName")]
        public string DatabaseName { get; set; }

        [JsonProperty(PropertyName = "DatabaseUser")]
        public string DatabaseUser { get; set; }

        [JsonProperty(PropertyName = "DatabasePass")]
        public string DatabasePass { get; set; }

        [JsonProperty(PropertyName = "Domain")]
        public string Domain { get; set; }

        public string ConnectionString()
        {

            return String.Format("mongodb://{2}:{3}@{0}/{1}",DatabaseServer ,DatabaseName ,DatabaseUser ,DatabasePass);
        }

        public static string initKey()
        {
            return ConfigurationManager.AppSettings["initKey"];
        }

        public void Save()
        {
            try
            {
                if (!Encrypt)
                {
                    System.IO.File.WriteAllText(ServerUtil.MapPath("~/App_Data/Config.txt"), JsonConvert.SerializeObject(this));
                }
                else
                {
                    System.IO.File.WriteAllText(ServerUtil.MapPath("~/App_Data/Config.txt"), Security.WopiSecurity.Encrypt(JsonConvert.SerializeObject(this), initKey()));
                }
            }
            catch(Exception ex)
            {
                ServerUtil.LogException(ex);
            }
        }

        private static void Clear()
        {
            try
            {
                System.IO.File.Delete(ServerUtil.MapPath("~/App_Data/Config.txt"));
            }
            catch
            {
            }
        }

        public static ConfigBag Open()
        {
            string configText;
            try
            {
                configText = System.IO.File.ReadAllText(ServerUtil.MapPath("~/App_Data/Config.txt"));
            }
            catch
            {
                Clear();
                return new ConfigBag();
            }

            try
            {
                return JsonConvert.DeserializeObject<ConfigBag>(Security.WopiSecurity.Decrypt(configText, initKey()));
            }
            catch
            {
                try
                {
                    return JsonConvert.DeserializeObject<ConfigBag>(configText);
                }
                catch
                {
                    Clear();
                    return new ConfigBag();
                }
            }
        }
    }

    public static class ServerUtil
    {
        public static void StartUp()
        {
            if (!Directory.Exists(ServerUtil.MapPath("~/App_Data/Log/"))) Directory.CreateDirectory(ServerUtil.MapPath("~/App_Data/Log/"));

            var clients = ClientIdentity.Select(id => true);
            if (clients.ToList().Count == 0)
            {
                var client = new ClientIdentity()
                {
                    ClientID = Guid.NewGuid().ToString().Replace("-", ""),
                    ClientSecret = Guid.NewGuid().ToString().Replace("-", ""),
                };
                client.Save();
            }
        }

        public static string MapPath(string Path)
        {
            return HttpContext.Current.Server.MapPath(Path);
        }

        private static string _storagePath = "";
        public static string StoragePath()
        {
            if (_storagePath == "")
            {
                _storagePath = ServerUtil.MapPath("~/App_Data/Storage/");
                if (!Directory.Exists(_storagePath)) Directory.CreateDirectory(_storagePath);
            }
            return _storagePath;
        }

        private static string _AuthKey = "";
        private static DateTime _AuthKeyExpires = DateTime.Now;
        public static string AuthenticationKey()
        {
            if(DateTime.Now>=_AuthKeyExpires || _AuthKey == "")
            {
                _AuthKey = Security.WopiSecurity.MD5Encrypt(Guid.NewGuid().ToString());
                _AuthKeyExpires = DateTime.Now.AddMinutes(10);
            }
            return _AuthKey;
        }

        public static void LogException(Exception ex)
        {
            if (!Directory.Exists(ServerUtil.MapPath("~/App_Data/Log/"))) Directory.CreateDirectory(ServerUtil.MapPath("~/App_Data/Log/"));

            var exceptionFile = ServerUtil.MapPath("~/App_Data/Log/Exception.log");
            System.IO.File.AppendAllText(exceptionFile, String.Format("{0} {1}\r\n{2}\r\n\r\n", DateTime.Now.ToString(), ex.Message, ex.StackTrace));
        }

        public static void Log(string Message)
        {
            if (!Directory.Exists(ServerUtil.MapPath("~/App_Data/Log/"))) Directory.CreateDirectory(ServerUtil.MapPath("~/App_Data/Log/"));

            var exceptionFile = ServerUtil.MapPath("~/App_Data/Log/Message.log");
            System.IO.File.AppendAllText(exceptionFile, String.Format("{0} {1}\r\n\r\n", DateTime.Now.ToString(), Message));
        }


        public static HttpResponseMessage returnStatus(HttpStatusCode code, string description)
        {
            HttpResponseMessage response = new HttpResponseMessage(code);
            response.ReasonPhrase = description;
            return response;
        }

        private static ConfigBag _config = null;
        public static ConfigBag Config()
        {
            if (_config == null)
            {
                _config = ConfigBag.Open();

                if (_config.Encrypt == true)
                {
                    _config.Save();
                }

                Token.Recycle();
            }
            return _config;
        }
    }
}