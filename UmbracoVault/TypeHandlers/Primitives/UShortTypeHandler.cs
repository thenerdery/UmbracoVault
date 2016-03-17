using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class UshortTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            ushort result;

            ushort.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}
        
        public Type TypeSupported => typeof (ushort);
    }
}
