using System;
using System.Collections.Generic;
using UmbracoVault.Collections;

namespace UmbracoVault.TypeHandlers
{
    public class GenericIListTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            var collection = new VaultItemCollection<T>(input.ToString());
            return collection;
        }

        public Type TypeSupported => typeof(IList<>);
    }
}
