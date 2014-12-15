using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class DoubleTypeHandler : ITypeHandler
    {
        private object Get(string stringValue)
        {
            double result;

            double.TryParse(stringValue, out result);

            return result;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

    	public Type TypeSupported
        {
            get { return typeof (double); }
        }
    }
}