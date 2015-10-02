using System;
using System.Collections.Generic;
using UmbracoVault.Collections;

namespace UmbracoVault.TypeHandlers
{
    public class GenericIListTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input) where T : class
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
