﻿using System.Web.Http;
using WebApiProxy.Server;

namespace WebApiAp.Configuration
{
    public static class WebApiApConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            //config.MapHttpAttributeRoutes();
            config.RegisterProxyRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/MoneySQ/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}