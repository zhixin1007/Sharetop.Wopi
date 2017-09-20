using Sharetop.Wopi.Models;
using Sharetop.Wopi.Models.Api;
using Sharetop.Wopi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Sharetop.Wopi.Security
{
    public class ApiProofValidationFilter : AuthorizeAttribute
    {
        /// <summary>
        /// Determines if the user is authorized to access the WebAPI endpoint based on the bearer token
        /// </summary>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            //return true;

            try
            {
                // Parse the query string and ensure there is an access_token
                var header = actionContext.Request.Headers;

                //string X_SWA_ClientID;
                string X_SWA_Proof;

                //if (!header.Contains("X-SWA-ClientID"))
                //{
                //    return false;
                //}
                //else
                //{
                //    X_SWA_ClientID =String.Join(",",header.GetValues("X-SWA-ClientID").ToArray());
                //}

                if (!header.Contains("X-SWA-Proof"))
                {
                    return false;
                }
                else
                {
                    X_SWA_Proof = String.Join(",", header.GetValues("X-SWA-Proof").ToArray());
                }

                try
                {

                    lock (ClientIdentity.Handle)
                    {
                        var clients = ClientIdentity.Select(id => id.Token == X_SWA_Proof).ToList();

                        if (clients.Count == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }

                        //var client = clients[0];
                        //if (clients[0].Token == X_SWA_Proof) //(WopiSecurity.MD5Encrypt(client.Token + client.Counter.ToString()) == header.GetValues("X-SWA-Proof").ToString())
                        //{
                        //    //client.Counter += 1;
                        //    //client.Save();
            
                        //    return true;
                        //}
                        //else
                        //{
                        //    return false;
                        //}
                    }

                }
                catch (Exception)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                // Any exception will return false, but should probably return an alternate status codes
                return false;
            }
        }
    }
}