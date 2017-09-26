using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Sharetop.Wopi.Models
{
    public class FileModel:mongoDocument
    {
        [JsonProperty(PropertyName = "OwnerId")]
        public string OwnerId { get; set; }

        [JsonProperty(PropertyName = "BaseFileName")]
        public string BaseFileName { get; set; }

        public string Extension()
        {
            return BaseFileName.Substring(BaseFileName.LastIndexOf('.') + 1).ToLower();
        }

        [JsonProperty(PropertyName = "Container")]
        public string Container { get; set; }

        [JsonProperty(PropertyName = "Size")]
        public long Size { get; set; }

        [JsonProperty(PropertyName = "Version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "UserId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "UserFriendlyName")]
        public string UserFriendlyName { get; set; }
    }
}