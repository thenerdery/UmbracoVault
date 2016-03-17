using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class DecimalTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            decimal result;

            decimal.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}
        
        public Type TypeSupported => typeof (decimal);
    }
}