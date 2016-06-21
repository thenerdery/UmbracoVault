using System;
using System.Collections.Generic;
using System.Reflection;
using Umbraco.Core.Models;

namespace UmbracoVault.Reflection
{
    public interface IContentInstanceFactory
    {
        /// <summary>
        /// Creates a new instance of type T.  If content is provided a constructor on the type taking 
        /// IPublishedContent will be attempted first.
        /// </summary>
        /// <param name="content">Content to use to populate instance</param>
        T CreateInstance<T>(IContent content);

        /// <summary>
        ///     Gets the properties that need to be filled on an instance
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        IList<PropertyInfo> GetPropertiesToFill<T>();

        /// <summary>
        ///     Gets the properties that need to be filled on an instance
        /// </summary>
        /// <param name="type">Type of instance</param>
        IList<PropertyInfo> GetPropertiesToFill(Type type);
    }
}
