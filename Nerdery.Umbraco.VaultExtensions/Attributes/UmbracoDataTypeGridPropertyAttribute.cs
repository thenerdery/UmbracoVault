using System;
using Nerdery.Umbraco.Vault.Attributes;
using Nerdery.Umbraco.VaultExtensions.TypeHandlers;

namespace Nerdery.Umbraco.VaultExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class UmbracoDataTypeGridPropertyAttribute : UmbracoPropertyAttribute
    {
        public UmbracoDataTypeGridPropertyAttribute()
        {
            TypeHandler = new DataTypeGridTypeHandler();
        }

        /// <summary>
        /// Only needed if the camelCase name of the property is not the same as the umbraco property alias
        /// </summary>
        /// <param name="name"></param>
        public UmbracoDataTypeGridPropertyAttribute(string name)
            : base(name)
        {
            TypeHandler = new DataTypeGridTypeHandler();
        }
    }
}
