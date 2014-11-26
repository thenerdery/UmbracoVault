using System;

namespace Nerdery.Umbraco.Vault.TypeHandlers.Primitives
{
    /// <summary>
    /// Responsible for converting string Types
    /// </summary>
    public class StringTypeHandler : ITypeHandler
    {
        private string Get(string stringValue)
        {
            return stringValue;
        }

        public object GetAsType<T>(object input)
        {
			return Get(input.ToString());
    	}

        public Type TypeSupported
        {
            get { return typeof (string); }
        }
    }
}