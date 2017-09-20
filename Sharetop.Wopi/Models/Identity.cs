using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sharetop.Wopi.Models
{
    public class Identity
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        public string Container()
        {
            return Name.Replace("@", "-").Replace(".", "-");
        }
    }
}