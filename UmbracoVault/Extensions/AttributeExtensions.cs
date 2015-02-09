using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UmbracoVault.Attributes;
using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Extensions
{
    public static class AttributeExtensions
    {
        public static IEnumerable<PropertyInfo> IsUmbracoProperties(this IEnumerable<PropertyInfo> properties)
        {
            var output = properties.Where(x => x.GetCustomAttributes(typeof(UmbracoPropertyAttribute), true).Any() && x.CanWrite);
            return output;
        }

        public static UmbracoPropertyAttribute GetUmbracoPropertyAttribute(this PropertyInfo propertyInfo)
        {
            var output = propertyInfo.GetCustomAttributes(typeof(UmbracoPropertyAttribute), true).FirstOrDefault() as UmbracoPropertyAttribute;
            return output;
        }

        public static string GetUmbracoPropertyName(this PropertyInfo propertyInfo, UmbracoPropertyAttribute attribute = null)
        {
            if (attribute == null)
            {
                attribute = propertyInfo.GetUmbracoPropertyAttribute();
            }

            string propertyName = string.IsNullOrWhiteSpace(attribute.Alias)
                                          ? string.Format("{0}{1}", propertyInfo.Name[0].ToString().ToLower(),
                                                          propertyInfo.Name.Substring(1)) //camel case the property name
                                          : attribute.Alias;

            return propertyName;
        }

        public static ITypeHandler GetTypeHander(this UmbracoPropertyAttribute attribute, Type propertyType)
        {
            var typeHandler = attribute.TypeHandler ?? TypeHandlerFactory.Instance.GetHandlerForType(propertyType);

            if (typeHandler == null)
            {
                throw new NotSupportedException(
                    string.Format("The property type {0} is not supported by Umbraco.Vault.", propertyType));
            }

            return typeHandler;
        }
    }
}
