using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class SByteTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            sbyte result;

            sbyte.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported => typeof(sbyte);
    }
}