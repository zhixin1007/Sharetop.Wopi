using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sharetop.Wopi.Utils;
using Sharetop.Wopi.Security;
using Sharetop.Wopi.Models;
using Sharetop.Wopi.Models.Api;

namespace Sharetop.Wopi.Controllers.Api
{
    public class AuthenticationController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            if (ServerUtil.Config().Domain == "")
            {
                ServerUtil.Config().Domain = Request.RequestUri.Authority;
                ServerUtil.Config().Save();
            }
            var response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
            response.Content = new StringContent(ServerUtil.AuthenticationKey());
            return response;
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(string id)
        {
            HttpResponseMessage response;
            switch (id.ToLower())
            {
                case "key":
                    response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");
                    response.Content = new StringContent(ServerUtil.AuthenticationKey());
                    break;
                default:
                    response = ServerUtil.returnStatus(HttpStatusCode.BadRequest, "Invalid Request");
                    break;
            }
            return response;
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]AuthInfo info)
        {
            try
            {
                var clients = ClientIdentity.Select(id => id.ClientID == info.ClientID).ToList();

                if (clients.Count == 0)
                {
                    return ServerUtil.returnStatus(HttpStatusCode.Unauthorized, "Authorization Failed");
                }

                var client = clients[0];
                if (WopiSecurity.MD5Encrypt(client.ClientSecret + ServerUtil.AuthenticationKey()) == info.SecureString)
                {
                    var response = ServerUtil.returnStatus(HttpStatusCode.OK, "Success");

                    client.Token = WopiSecurity.MD5Encrypt(Guid.NewGuid().ToString());
                    client.Counter = 1;

                    client.Save();

                    response.Content = new StringContent(client.Token);

                    return response;
                }
                else
                {
                    return ServerUtil.returnStatus(HttpStatusCode.Unauthorized, "Authorization Failed");
                }

            }
            catch (Exception ex)
            {
                ServerUtil.LogException(ex);
                return ServerUtil.returnStatus(HttpStatusCode.BadRequest, "Invalid Request");
            }
        }
    }
}