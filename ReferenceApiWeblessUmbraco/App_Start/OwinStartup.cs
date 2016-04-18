using System;
using Microsoft.Owin;
using Owin;
using ReferenceApiWeblessUmbraco.Application;
using ReferenceApiWeblessUmbraco.App_Start;

[assembly: OwinStartup(typeof(OwinStartup))]

namespace ReferenceApiWeblessUmbraco.App_Start
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            StartUmbracoContext();
        }

        private void StartUmbracoContext()
        {
            var application = new ReferenceApiApplicationBase();
            application.Start(application, new EventArgs());
        }
    }
}