using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Umbraco.Core;

namespace Nerdery.Umbraco.Vault.UmbracoApplication
{
    public class EventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            Vault.LockConfiguration();
            base.ApplicationStarted(umbracoApplication, applicationContext);
        }
    }
}
