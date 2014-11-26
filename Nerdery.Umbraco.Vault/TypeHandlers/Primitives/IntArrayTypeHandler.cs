using System;
using System.Collections.Generic;

namespace Nerdery.Umbraco.Vault.TypeHandlers.Primitives
{
    public class IntArrayTypeHandler : ITypeHandler
    {
        private object Get(string stringValue)
        {
            var intList = new List<int>();
            var stringArray = stringValue.Split(',');
            foreach(var item in stringArray)
            {
                int value;
                if (int.TryParse(item, out value))
                {
                    intList.Add(value);
                }
            }
            return intList.ToArray();
        }

        public object GetAsType<T>(object input)
        {
            return Get(input.ToString());
        }

        public Type TypeSupported
        {
            get
            {
                return typeof(int[]);
            }
        }
    }
}
