﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Cognition.Web.Helpers;

namespace Cognition.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Search",
                url: "search/{query}",
                defaults: new { controller = "Document", action = "Search", query = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Document",
                url: "d/{type}/{id}/{action}",
                defaults: new { controller = "Document", action = "Index", id = UrlParameter.Optional }
                );

            // using routes.MapRouteLowercase currently breaks Google sign in
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.LowercaseUrls = true;
        }
    }
}
