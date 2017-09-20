using System;
using Newtonsoft.Json;
using Sharetop.Wopi.Utils;
using Sharetop.Wopi.Security;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Sharetop.Wopi.Models.Api
{
    public class Token : mongoDocument
    {
        //public static Token Parse(string SecureString)
        //{
        //    var jsonString = WopiSecurity.Decrypt(SecureString);
        //    return JsonConvert.DeserializeObject<Token>(jsonString);
        //}
        [JsonProperty(PropertyName = "Identity")]
        public string Identity { get; set; }

        [JsonProperty(PropertyName = "UserId")]
        public string User { get; set; }

        [JsonProperty(PropertyName = "UserDisplayName")]
        public string UserDisplayName { get; set; }

        [JsonProperty(PropertyName = "ReadOnly")]
        public bool ReadOnly { get; set; }

        [JsonProperty(PropertyName = "Document")]
        public string Document { get; set; }

        [JsonProperty(PropertyName = "Expires")]
        public DateTime? Expires { get; set; }

        public DateTime ValidTo()
        {
            if (Expires == null)
            {
                return new DateTime(1970,1,1);
            }
            else
            {
                return (DateTime)Expires;
            }
        }

        //public string ToSecureString()
        //{
        //    var jsonString = JsonConvert.SerializeObject(this);
        //    var SecureString = WopiSecurity.Encrypt(jsonString);
        //    return SecureString;
        //}

        //public override string ToString()
        //{
        //    return JsonConvert.SerializeObject(this);
        //}

        public void Save()
        {
            if (DatabaseUtil<Token>.GetItems("Tokens", i => i.documentId == this.documentId).Count<Token>() > 0)
            {
                DatabaseUtil<Token>.UpdateItem("Tokens", this);
            }
            else
            {
                DatabaseUtil<Token>.CreateItem("Tokens", this);
            }
        }

        public static Token Create()
        {
            var t = new Token() {
                Identity = Guid.NewGuid().ToString().Replace("-", ""),
                Expires = DateTime.Now.AddHours(1),
            };
            t.Save();

            return t;
        }

        public static Token Invalid()
        {
            var t = new Token()
            {
                Expires = DateTime.Parse("1970-1-1"),
            };
            return t;
        }

        public static void Recycle()
        {
            DatabaseUtil<Token>.DeleteItems("Tokens",i=>i.Expires<DateTime.Now);
        }

        public static Token Get(string identity)
        {
            return DatabaseUtil<Token>.GetItem("Tokens", i => i.Identity == identity);
        }

        public static Token Get(Guid ID)
        {
            return DatabaseUtil<Token>.GetItem("Tokens", i => i.documentId == ID);
        }


        public static IEnumerable<Token> Select(Expression<Func<Token, bool>> predicate)
        {
            return DatabaseUtil<Token>.GetItems("Tokens", predicate);
        }
    }
}