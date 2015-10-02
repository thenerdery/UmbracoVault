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
        T CreateInstance<T>(IPublishedContent content) where T : class;
    }
}