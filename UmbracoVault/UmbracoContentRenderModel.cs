using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace UmbracoVault
{
    public class UmbracoContentRenderModel : RenderModel
    {
        public UmbracoContentRenderModel(IPublishedContent content) : base(content) { }
    }
}
