using System;
using System.Collections.Generic;
using System.Linq;

namespace UmbracoVault.Controllers
{
    /// <summary>
    ///     A controller to render front-end requests which completely bypasses the Umbraco RenderModel convention.
    /// </summary>
    [Obsolete("Use VaultRenderMvcController Instead")]
    public class VaultDefaultGenericController : VaultRenderMvcController
    {
    }
}