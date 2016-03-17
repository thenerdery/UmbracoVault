using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    /// <summary>
    /// Responsible for converting string Types
    /// </summary>
    public class StringTypeHandler : ITypeHandler
    {
        private static string Get(string stringValue)
        {
            return stringValue;
        }

        public object GetAsType<T>(object input)
        {
			return Get(input.ToString());
    	}

        public Type TypeSupported => typeof (string);
    }
}