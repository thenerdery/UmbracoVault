using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;

using UmbracoVault.Extensions;

namespace UmbracoVault.Reflection
{
    public class DefaultInstanceFactory : IInstanceFactory
    {
        public T CreateInstance<T>(IPublishedContent content, out bool fillProperties)
        {
            var targetType = typeof(T);
            var result = targetType.CreateWithContentConstructor<T>(content);

            if (result == null)
            {
                result = targetType.CreateWithNoParams<T>();
            }

            fillProperties = true;
            return result;
        }
    }
}