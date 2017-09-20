using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sharetop.Wopi.Models.Wopi
{
    public class WopiProof
    {
        public string oldvalue { get; set; }
        public string oldmodulus { get; set; }
        public string oldexponent { get; set; }
        public string value { get; set; }
        public string modulus { get; set; }
        public string exponent { get; set; }
    }
}