using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;

using UmbracoVault.Extensions;

namespace UmbracoVault.Reflection
{
    internal class DefaultInstanceFactory : IInstanceFactory
    {
        public T CreateInstance<T>(IPublishedContent content) where T : class
        {
            var targetType = typeof(T);
            var result = targetType.CreateWithContentConstructor<T>(content);

            if (result == null)
            {
                result = targetType.CreateWithNoParams<T>();
            }
            return result;
        }
    }
}