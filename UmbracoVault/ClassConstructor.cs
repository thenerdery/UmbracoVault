using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Umbraco.Core.Models;

using UmbracoVault.Extensions;
using UmbracoVault.Reflection;

namespace UmbracoVault
{
    /// <summary>
    ///     Invokes constructors for a given class
    /// </summary>
    public class ClassConstructor
    {
        private static IInstanceFactory _instanceFactory = new DefaultInstanceFactory();

        /// <summary>
        ///     Set new instance factory, enables proxy class hook
        /// </summary>
        public static void SetInstanceFactory(IInstanceFactory instanceFactory)
        {
            _instanceFactory = instanceFactory;
        }

        /// <summary>
        ///     Gets the properties that need to be filled on an instance
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        public static IList<PropertyInfo> GetPropertiesToFill<T>()
        {
            return _instanceFactory.GetPropertiesToFill<T>();
        }

        /// <summary>
        ///     Gets the properties that need to be filled on an instance
        /// </summary>
        /// <param name="type">Type of instance</param>
        public static IList<PropertyInfo> GetPropertiesToFill(Type type)
        {
            return _instanceFactory.GetPropertiesToFill(type);
        }

        public T CreateDefault<T>() where T : class, new()
        {
            return new T();
        }

        public static T CreateWithNode<T>(IPublishedContent content)
        {
            var result = _instanceFactory.CreateInstance<T>(content);
            SetPublishedContent(content, result);

            return result;
        }

        public static T CreateWithContent<T>(IContent content)
        {
            var result = _instanceFactory.CreateInstance<T>(content);
            SetContent(content, result);

            return result;
        }

        internal static void SetPublishedContent<T>(IPublishedContent content, T result)
        {
            var contentProperty = typeof(T).GetPublicSettableProperties()
                .FirstOrDefault(x => x.PropertyType.IsAssignableFrom(typeof(IPublishedContent)) && x.CanWrite);

            contentProperty?.SetValue(result, content);

            SetContentProperty(content, result);
        }

        internal static void SetContent<T>(IContent content, T result)
        {
            var contentProperty = typeof(T).GetPublicSettableProperties()
                .FirstOrDefault(x => x.PropertyType.IsAssignableFrom(typeof(IContent)) && x.CanWrite);

            contentProperty?.SetValue(result, content);
        }

        /// <summary>
        ///     If the target type inhereits from UmbracoContentModel, this sets the Content property
        /// </summary>
        private static void SetContentProperty<T>(IPublishedContent content, T result)
        {
            var model = result as UmbracoContentModel;
            if (model != null)
            {
                model.Content = content;
            }
        }

        internal static T CreateWithMember<T>(IMember member)
        {
            // since we're passing in NULL here, it doesn't matter which type we cast it to.
            var memberModel = _instanceFactory.CreateInstance<T>((IPublishedContent)null);

            var targetType = typeof(T);
            var memberProperty = targetType.GetPublicSettableProperties()
                .FirstOrDefault(x => x.PropertyType.IsAssignableFrom(typeof(IMember)) && x.CanWrite);

            memberProperty?.SetValue(memberModel, member);

            return memberModel;
        }
    }
}