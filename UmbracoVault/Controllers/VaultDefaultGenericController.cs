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
    [Obsolete("Use VaultRenderMvcController Instead")]
    public class VaultDefaultGenericController : VaultRenderMvcController
    {
    }
}