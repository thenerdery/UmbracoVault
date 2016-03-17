using System;
using System.Collections.Generic;
using System.Linq;

using ReferenceWebsite.TypeHandlers;

using Umbraco.Core;
using Umbraco.Web.Mvc;

using UmbracoVault;
using UmbracoVault.Controllers;
using UmbracoVault.TypeHandlers;

namespace ReferenceWebsite
{
    // ReSharper disable once UnusedMember.Global - Invoked via reflection
    public class VaultRegistrationEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            DefaultRenderMvcControllerResolver
                .Current
                .SetDefaultControllerType(typeof(VaultRenderMvcController));

            Vault.RegisterViewModelNamespace("ReferenceWebsite.Models", "ReferenceWebsite");
            TypeHandlerFactory.Instance.RegisterTypeHandler<LocationIdTypeHandler>();
        }
    }
}