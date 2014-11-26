using System;
using Nerdery.Umbraco.Vault.TypeHandlers;

namespace Nerdery.Umbraco.Vault.Attributes
{
    /// <summary>
    /// Denotes that a property is bound to an Umbraco property from a document
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class UmbracoRichTextPropertyAttribute : UmbracoPropertyAttribute
    {

        public UmbracoRichTextPropertyAttribute()
        {
            TypeHandler = new RichTextTypeHandler();
        }

        /// <summary>
        /// Only needed if the camelCase name of the property is not the same as the umbraco property alias
        /// </summary>
        /// <param name="name"></param>
        public UmbracoRichTextPropertyAttribute(string name) : base(name)
        {
            TypeHandler = new RichTextTypeHandler();
        }
    }
}