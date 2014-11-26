using System;
using Nerdery.Umbraco.Vault.TypeHandlers;

namespace Nerdery.Umbraco.Vault.Attributes
{
    /// <summary>
    /// Denotes that a property is bound to an Umbraco property from a document
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class UmbracoPropertyAttribute : Attribute
    {

        public UmbracoPropertyAttribute()
        {
        }

        /// <summary>
        /// Only needed if the camelCase name of the property is not the same as the umbraco property alias
        /// </summary>
        /// <param name="alias"></param>
        public UmbracoPropertyAttribute(string alias)
        {
            Alias = alias;
        }

        /// <summary>
        /// The alias name of the property field. Expected to be camelcase.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Set to true to recursively find the property value
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// Declares a specific type handler to use for getting/setting this property.
        /// </summary>
        public ITypeHandler TypeHandler { get; set; }
    }
}