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
                name: "news",
                url: "news/{title}",
                defaults: new { controller = "Home", action = "NewsDetails" }
            );

            routes.MapRoute(
                name: "help",
                url: "help/{name}",
                defaults: new { controller = "Home", action = "NeedHelpDetails" }
            );

            routes.MapRoute(
                name: "projects",
                url: "projects/{title}",
                defaults: new { controller = "Home", action = "ProjectDetails" }
            );

            routes.MapRoute(
                name: "newsIndex",
                url: "news",
                defaults: new { controller = "Home", action = "NewsIndex" }
            );

            routes.MapRoute(
                name: "helpIndex",
                url: "help",
                defaults: new { controller = "Home", action = "NeedHelpIndex" }
            );

            routes.MapRoute(
                name: "projectsIndex",
                url: "projects",
                defaults: new { controller = "Home", action = "ProjectsIndex" }
            );


            routes.MapRoute(
    name: "Admin",
    url: "admin/{controller}/{action}/{id}",
    defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
);


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}