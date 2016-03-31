using System;

using UmbracoVault.Extensions;
using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Attributes
{
    /// <summary>
    /// Denotes that the property contains JSON data and should be deserialized into a specific object type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UmbracoJsonPropertyAttribute : UmbracoPropertyAttribute
    {
        /// <summary>
        /// Creates an UmbracoJsonPropertyAttribute
        /// </summary>
        /// <param name="objectType">Type of object to be deserialized into</param>
        public UmbracoJsonPropertyAttribute(Type objectType)
        {
            var typeHandler = typeof(JsonDataTypeHandler<>).MakeGenericType(objectType);
            TypeHandler = typeHandler.CreateWithNoParams<ITypeHandler>();
        }
    }
}