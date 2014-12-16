using System;
using System.Web;

namespace UmbracoVault.TypeHandlers
{
    /// <summary>
    /// Responsible for converting HtmlString Types
    /// </summary>
    public class HtmlStringTypeHandler : ITypeHandler
    {
        private HtmlString Get(string stringValue)
        {
            return new HtmlString(stringValue);
        }

        public object GetAsType<T>(object input)
        {
			return Get(input.ToString());
    	}

        public Type TypeSupported
        {
            get { return typeof (HtmlString); }
        }
    }
}