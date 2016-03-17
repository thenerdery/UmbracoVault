using System;
using System.Collections.Generic;
using System.Linq;

using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Tests.Models
{
    public class AutoRegisteredTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            throw new NotImplementedException();
        }

        public Type TypeSupported => typeof(AutoRegisteredType);
    }

    public class AutoRegisteredType
    {
    }
}