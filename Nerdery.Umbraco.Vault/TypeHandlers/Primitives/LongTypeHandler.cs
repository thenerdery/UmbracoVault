using System;

namespace Nerdery.Umbraco.Vault.TypeHandlers.Primitives
{
    public class UlongTypeHandler : ITypeHandler
    {
        private object Get(string stringValue)
        {
            ulong result;

            ulong.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported
        {
            get { return typeof (ulong); }
        }
    }
}
