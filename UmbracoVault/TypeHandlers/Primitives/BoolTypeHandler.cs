using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class BoolTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            bool result;

            if (!bool.TryParse(stringValue, out result))
            {
                // Umbraco True/false data types map to 1 and 0 respectively
                if (stringValue == "1") result = true;
            }

            return result;
        }

    	public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported => typeof(bool);
    }
}