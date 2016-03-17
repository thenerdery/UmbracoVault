using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class ShortTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            short result;

            short.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported => typeof (short);
    }
}
