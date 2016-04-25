using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using Umbraco.Core.Models;

using UmbracoVault.Attributes;

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
        /// Attempts to create T by supplying the IContent to a constructor (if one exists with that parameter signature)
        /// Returns null if no appropriate constructor exists.
        /// </summary>
        internal static T CreateWithContentConstructor<T>(this Type targetType, IContent content)
        {
            var nodeConstructor = targetType.GetConstructor(new[] { typeof(IContent) });
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

        public static ReadOnlyCollection<UmbracoEntityAttribute> GetUmbracoEntityAttributes(this Type type)
        {
            var result = new List<UmbracoEntityAttribute>();
            var attributes = type.GetCustomAttributes(typeof(UmbracoEntityAttribute), inherit: true) as UmbracoEntityAttribute[];
            if (attributes != null)
            {
                result.AddRange(attributes);
            }
            return result.AsReadOnly();
        }

        public static IList<PropertyInfo> GetDefaultPropertiesToFill(this Type type)
        {
            IList<PropertyInfo> properties;
            var optionAttribute = type.GetUmbracoEntityAttributes().FirstOrDefault();
            if (optionAttribute != null && optionAttribute.AutoMap)
            {
                properties = GetAllPropertiesExceptOptedOut(type);
            }
            else
            {
                properties = GetPropertiesOptedInForMapping(type);
            }

            return properties;
        }

        public static IEnumerable<PropertyInfo> GetPublicSettableProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Gets properties that are decorated with [UmbracoProperty] (opt-in mode)
        /// </summary>
        private static IList<PropertyInfo> GetPropertiesOptedInForMapping(Type type)
        {
            return type.GetPublicSettableProperties()
                .Where(
                    x =>
                        x.GetCustomAttributes(typeof(UmbracoPropertyAttribute), true)
                            .Any() && x.CanWrite).ToList();
        }

        /// <summary>
        /// Gets properties that are NOT decorated with [UmbracoIgnoreProperty] (opt-out mode)
        /// </summary>
        private static IList<PropertyInfo> GetAllPropertiesExceptOptedOut(Type type)
        {
            return type.GetPublicSettableProperties()
                .Where(
                    x =>
                        !x.GetCustomAttributes(typeof(UmbracoIgnorePropertyAttribute), true)
                            .Any() && x.CanWrite
                            && x.PropertyType != typeof(IPublishedContent)).ToList();
        }
    }
}
