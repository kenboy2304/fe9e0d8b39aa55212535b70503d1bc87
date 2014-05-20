using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cdnvn.Phim.Web
{
    public class AllPathRouteHandler : MvcRouteHandler
    {
        private readonly string key;

        public AllPathRouteHandler(string key)
        {
            this.key = key;
        }

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var allPaths = requestContext.RouteData.Values[key] as string;
            if (!string.IsNullOrEmpty(allPaths))
            {
                requestContext.RouteData.Values[key] = allPaths.Split('/');
            }
            return base.GetHttpHandler(requestContext);
        }
    }
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
               name: "Account",
               url: "tai-khoan/{action}/{id}",
               defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Film",
               url: "film/{action}/{id}",
               defaults: new { controller = "Film", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Html",
                url: "{keyword}.html",
                defaults: new { controller = "Film", action = "FilmDetails", keyword = UrlParameter.Optional, path = UrlParameter.Optional }
            );
            routes.Add(new Route("{*keyword}",
                new RouteValueDictionary(
                        new
                        {
                            controller = "Category",
                            action = "CategoryDetails",
                        }),
                new AllPathRouteHandler("keyword")));

            //routes.MapRoute(
            //    name: "Keywords",
            //    url: "{*keyword}.{type}",
            //    defaults: new { controller = "Home", action = "Details", keyword = UrlParameter.Optional, type = UrlParameter.Optional }
            //);
            //routes.MapRoute(
            //    name: "Keywords",
            //    url: "{keyword}.{type}",
            //    defaults: new { controller = "Home", action = "Details", keyword = UrlParameter.Optional, type = UrlParameter.Optional }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}