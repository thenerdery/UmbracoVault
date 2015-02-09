using System;
using UmbracoVault.Attributes;
using UmbracoVault.Extensions;

namespace UmbracoVault.TypeHandlers
{
    /// <summary>
    /// Used for binding WYSIWYG fields in Umbraco. Executes Macros within Rich Text Fields.
    /// </summary>
    [IgnoreTypeHandlerAutoRegistration]
    public class RichTextTypeHandler : ITypeHandler
    {
        private object Get(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue)) return stringValue;

            return stringValue.RenderLinks().ProcessUmbracoMacros();
        }

    	public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported
        {
            get { return typeof (string); }
        }
    }
}