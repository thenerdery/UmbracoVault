using System;

namespace Nerdery.Umbraco.Vault.TypeHandlers.Primitives
{
    public class DecimalTypeHandler : ITypeHandler
    {
        private object Get(string stringValue)
        {
            decimal result;

            decimal.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}
        
        public Type TypeSupported
        {
            get { return typeof (decimal); }
        }

    }
}