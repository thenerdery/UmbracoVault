using System;

namespace Nerdery.Umbraco.Vault.TypeHandlers.Primitives
{
    public class ShortTypeHandler : ITypeHandler
    {
        private object Get(string stringValue)
        {
            short result;

            short.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported
        {
            get { return typeof (short); }
        }
    }
}
