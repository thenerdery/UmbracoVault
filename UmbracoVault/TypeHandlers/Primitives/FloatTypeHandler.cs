using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class FloatTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            float result;

            float.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported => typeof (float);
    }
}