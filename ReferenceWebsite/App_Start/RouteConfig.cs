using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReferenceWebsite
{
    internal class RouteConfig
    {
        /// <summary>
        ///     Register Umbraco application routes.
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Performance Route",
                "Performance/{action}",
                new { controller = "Performance", action = "RunProxyPerfTests" }
                );

            routes.MapRoute(
                "Proxy Tests",
                "ProxyTests/{action}",
                new { controller = "ProxyTests", action = "ValueShouldMatchCmsContent" }
                );
        }
    }
}