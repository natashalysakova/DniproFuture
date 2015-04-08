using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DniproFuture
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "event",
                url: "event/{title}",
                defaults: new { controller = "Home", action = "NewsDetails" }
            );

            routes.MapRoute(
                name: "help",
                url: "help/{name}",
                defaults: new { controller = "Home", action = "NeedHelpDetails" }
            );

            routes.MapRoute(
                name: "newsIndex",
                url: "event",
                defaults: new { controller = "Home", action = "NewsIndex" }
            );

            routes.MapRoute(
                name: "helpIndex",
                url: "help",
                defaults: new { controller = "Home", action = "NeedHelpIndex" }
            );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}