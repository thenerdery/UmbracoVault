using System;

namespace Nerdery.Umbraco.Vault.TypeHandlers.Primitives
{
    public class CharTypeHandler : ITypeHandler
    {
        #region ITypeHandler Members

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

    	public Type TypeSupported
        {
            get { return typeof(char); }
        }

        #endregion
    }
}