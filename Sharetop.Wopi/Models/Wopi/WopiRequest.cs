using Sharetop.Wopi.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sharetop.Wopi.Models.Wopi
{
    public class WopiRequest
    {
        public string Id { get; set; }
        public WopiRequestType RequestType { get; set; }
        public Token AccessToken { get; set; }
    }
}