using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sharetop.Wopi.Models
{
    public abstract class mongoDocument
    {
        [NonSerialized]
        public static Object Handle = new Object();

        public mongoDocument()
        {
            documentId = Guid.NewGuid();
        }

        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }

        [JsonIgnore]
        public Guid documentId { get; set; }
    }
}