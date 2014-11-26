using System;

namespace Nerdery.Umbraco.Vault.TypeHandlers.Primitives
{
    public class IntTypeHandler : ITypeHandler
    {
        private object Get(string stringValue)
        {
            int result;

            int.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

    	public Type TypeSupported
        {
            get { return typeof (int); }
        }
    }
}
