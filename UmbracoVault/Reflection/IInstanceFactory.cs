using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;

namespace UmbracoVault.Reflection
{
    public interface IInstanceFactory
    {
        /// <summary>
        /// Creates a new instance of type T.  If content is provided a constructor on the type taking 
        /// IPublishedContent will be attempted first.
        /// </summary>
        /// <param name="content">Content to use to populate instance</param>
        /// <param name="enableFillProperties">True if instance properties should be populated, false if this step should be skipped</param>
        T CreateInstance<T>(IPublishedContent content, out bool enableFillProperties);
    }
}