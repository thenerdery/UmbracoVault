
using System;
using System.Linq;
using System.Reflection;

using UmbracoVault.Extensions;

using Umbraco.Core.Models;

using UmbracoVault.Reflection;

namespace UmbracoVault
{
    /// <summary>
    /// Invokes constructors for a given class
    /// </summary>
    internal class ClassConstructor
    {
        private static IInstanceFactory _instanceFactory = new DefaultInstanceFactory();

        /// <summary>
        /// Set new instance factory, enables proxy class hook
        /// </summary>
        public static void SetInstanceFactory(IInstanceFactory instanceFactory)
        {
            _instanceFactory = instanceFactory;
        }

        public T CreateDefault<T>() where T : class, new()
        {
            return new T();
        }

        public static T CreateWithNode<T>(IPublishedContent content) where T : class
        {
            var result = _instanceFactory.CreateInstance<T>(content);
            SetPublishedContent(content, result);

            return result;
        }

        internal static void SetPublishedContent<T>(IPublishedContent content, T result)
        {
            var contentProperty = typeof(T).GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(x => x.PropertyType.IsAssignableFrom(typeof(IPublishedContent)) && x.CanWrite);

            if (contentProperty != null)
            {
                contentProperty.SetValue(result, content);
            }

            SetContentProperty(content, result);
        }

        /// <summary>
        /// If the target type inhereits from UmbracoContentModel, this sets the Content property
        /// </summary>
        private static void SetContentProperty<T>(IPublishedContent content, T result)
        {
            var model = result as UmbracoContentModel;
            if (model != null)
            {
                model.Content = content;
            }
        }

        public T CreateWithMember<T>(IMember member) where T : class
        {
            var memberModel = _instanceFactory.CreateInstance<T>(null);

            var targetType = typeof(T);
            var memberProperty = targetType.GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(x => x.PropertyType.IsAssignableFrom(typeof(IMember)) && x.CanWrite);

            if (memberProperty != null)
            {
                memberProperty.SetValue(memberModel, member);
            }

            return memberModel;

        }

    }
}