using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IT_BookTrade
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{option}/{id}",
                defaults: new { option = RouteParameter.Optional, id = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "ChatApi2",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
