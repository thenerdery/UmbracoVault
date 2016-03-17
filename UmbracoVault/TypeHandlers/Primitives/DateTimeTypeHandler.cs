using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class DateTimeTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            DateTime result;

            DateTime.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}
        
        public Type TypeSupported => typeof (DateTime);
    }
}