using System;

namespace Nerdery.Umbraco.Vault.TypeHandlers.Primitives
{
    public class ByteTypeHandler : ITypeHandler
    {
        private object Get(string stringValue)
        {
            byte result;

            byte.TryParse(stringValue, out result);

            return result;
        }

    	public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

    	public Type TypeSupported
        {
            get { return typeof(byte); }
        }
    }
}