using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class LongTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            long result;

            long.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported => typeof (long);
    }
}
