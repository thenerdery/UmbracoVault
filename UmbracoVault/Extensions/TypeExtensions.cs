using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;

namespace UmbracoVault.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Attempts to create T by supplying the IPublishedContent to a constructor (if one exists with that parameter signature)
        /// Returns null if no appropriate constructor exists.
        /// </summary>
        internal static T CreateWithContentConstructor<T>(this Type targetType, IPublishedContent content)
        {
            var nodeConstructor = targetType.GetConstructor(new[] { typeof(IPublishedContent) });
            if (nodeConstructor != null)
            {
                var result = (T)nodeConstructor.Invoke(new object[] { content });
                return result;
            }
            return default(T);
        }

        /// <summary>
        /// Attempts to create T with a parameterless constructor.
        /// Returns null if no appropriate constructor exists.
        /// </summary>
        /// <remarks>Meant for internal use and use by extensions.</remarks>
        public static T CreateWithNoParams<T>(this Type targetType)
        {
            
            var nodeConstructor = targetType.GetConstructor(Type.EmptyTypes);
            if (nodeConstructor != null)
            {
                var result = (T)nodeConstructor.Invoke(null);
                return result;
            }
            return default(T);
        } 
    }
}
