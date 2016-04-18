using System;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using ReferenceApiWeblessUmbraco.Application;
using ReferenceApiWeblessUmbraco.App_Start;
using umbraco.presentation.channels.businesslogic;
using UmbracoVault;

[assembly: OwinStartup(typeof(OwinStartup))]

namespace ReferenceApiWeblessUmbraco.App_Start
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            StartUmbracoContext();

            app.UseWebApi(config);
        }

        private void StartUmbracoContext()
        {
            var application = new ReferenceApiApplicationBase();
            application.Start(application, new EventArgs());

            Vault.SetOverrideContext(new UmbracoWeblessContext());
        }
    }
}