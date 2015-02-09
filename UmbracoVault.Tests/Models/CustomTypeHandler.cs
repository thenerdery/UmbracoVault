using System;
using System.Collections.Generic;
using System.Linq;

using UmbracoVault.Attributes;
using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Tests.Models
{
    [IgnoreTypeHandlerAutoRegistration]
    internal class CustomTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            throw new NotImplementedException();
        }

        public Type TypeSupported
        {
            get { return typeof(ExampleModelAllTypes); }
        }
    }
}