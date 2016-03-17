using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UmbracoVault.Proxy
{
    internal interface ILazyResolverMixin
    {
        object GetOrResolve(string alias, PropertyInfo propertyInfo);
    }
}