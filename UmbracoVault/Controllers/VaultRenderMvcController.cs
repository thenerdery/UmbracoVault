using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using log4net;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using UmbracoVault.Exceptions;

namespace UmbracoVault.Controllers
{
    public class VaultRenderMvcController : RenderMvcController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (VaultRenderMvcController));

        public VaultRenderMvcController()
        {
        }

        public VaultRenderMvcController(UmbracoContext umbracoContext) : base(umbracoContext)
        {
        }

        public ActionResult Index()
        {
            return CurrentTemplate();
        }

        /// <summary>
        ///     The default action to render the front-end view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override ActionResult Index(RenderModel model)
        {
            return CurrentTemplate(model);
        }

        /// <summary>
        ///     Returns an ActionResult based on the template name found in the route values and the given model.
        /// </summary>
        /// <returns></returns>
        protected new ActionResult CurrentTemplate<T>(T model)
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

            if (model == null)
            {
                throw new ViewModelNotFoundException(template, checkedTypes);
            }

            return View(template, model);
        }

        private ActionResult CurrentTemplate()
        {
            return CurrentTemplate<object>(null);
        }
    }
}