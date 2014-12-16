using System;
using UmbracoVault.Attributes;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class DateTimeTypeHandler : ITypeHandler
    {
        private object Get(string stringValue)
        {
            DateTime result;

            DateTime.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}
        
        public Type TypeSupported
        {
            get { return typeof (DateTime); }
        }
    }
}