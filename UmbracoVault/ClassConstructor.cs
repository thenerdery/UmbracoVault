
using System;
using System.Linq;
using System.Reflection;

using UmbracoVault.Extensions;

using Umbraco.Core.Models;

namespace UmbracoVault
{
    /// <summary>
    /// Invokes constructors for a given class
    /// </summary>
    internal class ClassConstructor
    {

        public T CreateDefault<T>() where T : class, new()
        {
            return new T();
        }

        public T CreateWithNode<T>(IPublishedContent content)
        {
            var targetType = typeof(T);
            var result = targetType.CreateWithContentConstructor<T>(content);

            if (result == null)
            {
                result = targetType.CreateWithNoParams<T>();
            }

            var contentProperty = typeof(T).GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(x => x.PropertyType.IsAssignableFrom(typeof(IPublishedContent)) && x.CanWrite);

            if (contentProperty != null)
            {
                contentProperty.SetValue(result, content);
            }

            SetContentProperty(content, result);

            return result;
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

    }
}