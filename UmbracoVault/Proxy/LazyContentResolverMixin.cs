using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Umbraco.Core.Models;

namespace UmbracoVault.Proxy
{
    public class LazyContentResolverMixin : ILazyResolverMixin
    {
        private readonly IContent _content;
        private readonly Dictionary<string, object> _valueCache = new Dictionary<string, object>();
        private readonly IUmbracoContext _umbracoContext;

        public LazyContentResolverMixin(IContent content)
        {
            _content = content;
            _umbracoContext = Vault.Context;
        }

        public object GetOrResolve(string alias, PropertyInfo propertyInfo)
        {
            object value;
            if (!_valueCache.TryGetValue(alias, out value))
            {
                _umbracoContext.TryGetValueForProperty(
                    (propAlias, recursive) => ResolveValue(propAlias, recursive, _content),
                    propertyInfo,
                    out value);

                _valueCache.Add(alias, value);
            }
            else
            {
                Console.WriteLine("From cache");
            }

            return value;
        }

        private static object ResolveValue(string alias, bool recursive, IContent node)
        {
            if (node.HasProperty(alias))
            {
                var value = node.GetValue(alias);
                return value;
            }

            return null;
        }
    }
}