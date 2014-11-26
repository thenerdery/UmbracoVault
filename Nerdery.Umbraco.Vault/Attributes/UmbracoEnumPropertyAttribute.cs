using System;
using Nerdery.Umbraco.Vault.TypeHandlers;

namespace Nerdery.Umbraco.Vault.Attributes
{
    /// <summary>
    /// Denotes that a property is bound to an Umbraco property from a document
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class UmbracoEnumPropertyAttribute : UmbracoPropertyAttribute
    {

        public UmbracoEnumPropertyAttribute()
        {
            TypeHandler = new EnumTypeHandler();
        }

        /// <summary>
        /// Only needed if the camelCase name of the property is not the same as the umbraco property alias
        /// </summary>
        /// <param name="name"></param>
		public UmbracoEnumPropertyAttribute(string name)
			: base(name)
        {
			TypeHandler = new EnumTypeHandler();
        }
    }
}