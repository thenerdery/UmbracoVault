using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class UlongTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            ulong result;

            ulong.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported => typeof (ulong);
    }
}
