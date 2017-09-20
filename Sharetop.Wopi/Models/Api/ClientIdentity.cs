using Newtonsoft.Json;
using Sharetop.Wopi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Sharetop.Wopi.Models.Api
{
    public class AuthInfo
    {
        public string ClientID { get; set; }
        public string SecureString { get; set; }
    }

    public class ClientIdentity:mongoDocument
    {
        public string ClientID { get; set; }

        public string ClientSecret { get; set; }

        public string Token { get; set; }

        public int Counter { get; set; }


        public void Save()
        {
            if (DatabaseUtil<ClientIdentity>.GetItems("ClientIdentity",i=>i.documentId == this.documentId).Count<ClientIdentity>() > 0)
            {
                DatabaseUtil<ClientIdentity>.UpdateItem("ClientIdentity", this);
            }
            else
            {
                DatabaseUtil<ClientIdentity>.CreateItem("ClientIdentity", this);
            }
        }

        public static ClientIdentity Get(Guid ID)
        {
            return DatabaseUtil<ClientIdentity>.GetItem("ClientIdentity", i => i.documentId == ID);
        }


        public static IEnumerable<ClientIdentity> Select(Expression<Func<ClientIdentity, bool>> predicate)
        {
            return DatabaseUtil<ClientIdentity>.GetItems("ClientIdentity", predicate);
        }
    }
}