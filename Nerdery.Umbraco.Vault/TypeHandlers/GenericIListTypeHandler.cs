using System;
using System.Collections.Generic;
using Nerdery.Umbraco.Vault.Collections;

namespace Nerdery.Umbraco.Vault.TypeHandlers
{
    public class GenericIListTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            var collection = new VaultItemCollection<T>(input.ToString());
            return collection;
        }

        public Type TypeSupported
        {
            get
            {
                return typeof(IList<>);
            }
        }
    }
}
