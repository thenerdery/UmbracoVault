using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Umbraco.Core.Models;

using UmbracoVault.Extensions;

namespace UmbracoVault.Reflection
{
    public class DefaultInstanceFactory : IInstanceFactory
    {
        public T CreateInstance<T>(IPublishedContent content)
        {
            var targetType = typeof(T);
            var result = targetType.CreateWithContentConstructor<T>(content);

            if (result == null)
            {
                result = targetType.CreateWithNoParams<T>();
            }

            return result;
        }

        public T CreateInstance<T>(IContent content)
        {
            var targetType = typeof(T);
            var result = targetType.CreateWithContentConstructor<T>(content);

            if (result == null)
            {
                result = targetType.CreateWithNoParams<T>();
            }

            return result;
        }

        public IList<PropertyInfo> GetPropertiesToFill<T>()
        {
            return GetPropertiesToFill(typeof(T));
        }

        public IList<PropertyInfo> GetPropertiesToFill(Type type)
        {
            return type.GetDefaultPropertiesToFill();
        }
    }
}