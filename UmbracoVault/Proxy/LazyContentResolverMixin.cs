using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Umbraco.Core.Models;

namespace UmbracoVault.Proxy
{
    public class LazyContentResolverMixin : ILazyResolverMixin
    {
        private readonly IContent _content;
        private readonly IUmbracoContext _umbracoContext;
        private readonly ConcurrentDictionary<string, object> _valueCache = new ConcurrentDictionary<string, object>();

        public LazyContentResolverMixin(IContent content)
        {
            _content = content;
            _umbracoContext = Vault.Context;
        }

        public object GetOrResolve(string alias, PropertyInfo propertyInfo)
        {
            return _valueCache.GetOrAdd(alias, key =>
            {
                object value;
                _umbracoContext.TryGetValueForProperty(
                    (propAlias, recursive) => ResolveValue(propAlias, recursive, _content),
                    propertyInfo,
                    out value);

                return value;
            });
        }

        public void Set(string alias, object value)
        {
            _valueCache.AddOrUpdate(alias, key => value, (key, oldValue) => value);
        }

        private static object ResolveValue(string alias, bool recursive, IContent node)
        {
            var value = node.HasProperty(alias) ? node.GetValue(alias) : null;
            if (recursive)
            {
                node = node.Parent();
                while (node != null && value == null)
                {
                    value = node.HasProperty(alias) ? node.GetValue(alias) : null;
                    node = node.Parent();
                }
            }

            return value;
        }
    }
}