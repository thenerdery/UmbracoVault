using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class UIntTypeHandler : ITypeHandler
    {
        private object Get(string stringValue)
        {
            uint result;

            uint.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported
        {
            get { return typeof (uint); }
        }
    }
}
