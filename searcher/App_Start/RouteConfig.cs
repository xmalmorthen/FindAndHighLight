using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace searcher
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Searcher",
                "Searcher",
                new { controller = "Home", action = "Index2", text = UrlParameter.Optional }
            );

            routes.MapRoute(
                "UploadFiles",
                "UploadFiles",
                new { controller = "Home", action = "filesToSearch" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "filesToSearch", id = UrlParameter.Optional }
            );
        }
    }
}