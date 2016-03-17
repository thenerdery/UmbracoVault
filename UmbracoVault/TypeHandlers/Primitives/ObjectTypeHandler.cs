using System;

namespace UmbracoVault.TypeHandlers.Primitives
{
    public class ObjectTypeHandler : ITypeHandler
    {
        private static object Get(string stringValue)
        {
            return stringValue;
        }

        public object GetAsType<T>(object input)
    	{
			return Get(input.ToString());
    	}

        public Type TypeSupported => typeof(object);
    }
}