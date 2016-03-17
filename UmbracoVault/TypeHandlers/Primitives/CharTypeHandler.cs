using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class CharTypeHandler : ITypeHandler
    {
        public object Get(string stringValue)
        {
            char result;

            char.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

    	public Type TypeSupported => typeof(char);
    }
}