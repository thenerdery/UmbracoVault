using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using UmbracoVault.Exceptions;

using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace UmbracoVault.Controllers
{
    // TODO: update comments to further reflect functionality
    /// <summary>
    /// A controller to render front-end requests which completely bypasses the Umbraco RenderModel convention.
    /// </summary>
    public class VaultDefaultGenericController : RenderMvcController
    {
        public VaultDefaultGenericController() { }
        public VaultDefaultGenericController(UmbracoContext umbracoContext) : base(umbracoContext) { }

        /// <summary>
        /// Returns an ActionResult based on the template name found in the route values and the given model.
        /// </summary>
        /// <returns></returns>
        protected ActionResult InferredTemplate()
        {
            var template = ControllerContext.RouteData.Values["action"].ToString();
            if (!EnsurePhsyicalViewExists(template))
            {
                throw new TemplateFileNotFoundException(template);
            }
            var checkedTypes = new List<string>();
            foreach (var nsTemplate in Vault.GetRegisteredNamespaces())
            {
                var viewModelTypeString = string.Format(nsTemplate, template);
                checkedTypes.Add(viewModelTypeString);
                var type = Type.GetType(viewModelTypeString);
                if (type != null)
                {
                    var inferredViewModel = Vault.Context.GetCurrent(type);
                    if (inferredViewModel != null)
                    {
                        return View(template, inferredViewModel);
                    }
                }
            }

            throw new ViewModelNotFoundException(template, checkedTypes);
        }

        public ActionResult Index()
        {
            return InferredTemplate();
        }

    }
}