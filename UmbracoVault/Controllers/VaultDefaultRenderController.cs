using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace UmbracoVault.Controllers
{
    // TODO: update comments to reflect functionality
    /// <summary>
    /// A controller to render front-end requests
    /// </summary>
    public class VaultDefaultRenderController : RenderMvcController
    {
        public VaultDefaultRenderController() { }
        public VaultDefaultRenderController(UmbracoContext umbracoContext) : base(umbracoContext) { }

        /// <summary>
        /// Returns an ActionResult based on the template name found in the route values and the given model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// If the template found in the route values doesn't physically exist, then an empty ContentResult will be returned.
        /// </remarks>
        protected new ActionResult CurrentTemplate<T>(T model)
        {
            var template = ControllerContext.RouteData.Values["action"].ToString();
            if (!EnsurePhsyicalViewExists(template))
            {
                return Content("");
            }

            foreach (var nsTemplate in Vault.GetRegisteredNamespaces())
            {
                var viewModelTypeString = string.Format(nsTemplate, template);
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
            
            return View(template, model);
        }

        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override ActionResult Index(RenderModel model)
        {
            return CurrentTemplate(model);
        }

    }
}