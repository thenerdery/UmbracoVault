using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UmbracoVault.Proxy
{
    public interface ILazyResolverMixin
    {
        T GetOrResolve<T>(string alias, PropertyInfo propertyInfo);
    }
}