using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class IntTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            int result;

            int.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

    	public Type TypeSupported => typeof (int);
    }
}
