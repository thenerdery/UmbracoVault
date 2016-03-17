using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

using Umbraco.Web;

namespace ReferenceWebsite
{
    public class Global : UmbracoApplication
    {
        protected override void OnApplicationStarting(object sender, System.EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}