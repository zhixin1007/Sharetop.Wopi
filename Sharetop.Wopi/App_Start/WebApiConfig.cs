using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Sharetop.Wopi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "WopiApi",
                routeTemplate: "wopi/{controller}/{id}/{action}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "StorageApi",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
            );

        }
    }
}
