using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Umbraco.Core.Models;
using Umbraco.Web;

namespace UmbracoVault.Proxy
{
    /// <summary>
    ///     Mixed into proxy object to provide lazy loading of values based on data provided by IPublishedContent node
    /// </summary>
    public class LazyResolverMixin : ILazyResolverMixin
    {
        private readonly IPublishedContent _node;
        private readonly IUmbracoContext _umbracoContext;
        private readonly ConcurrentDictionary<string, object> _valueCache = new ConcurrentDictionary<string, object>();

        public LazyResolverMixin(IPublishedContent node)
        {
            _node = node;
            _umbracoContext = Vault.Context;
        }

        public object GetOrResolve(string alias, PropertyInfo propertyInfo)
        {
            return _valueCache.GetOrAdd(alias, key =>
            {
                object value;
                _umbracoContext.TryGetValueForProperty(
                    (propAlias, recursive) => ResolveValue(propAlias, recursive, _node),
                    propertyInfo,
                    out value);

                return value;
            });
        }

        public void Set(string alias, object value)
        {
            _valueCache.AddOrUpdate(alias, key => value, (key, oldValue) => value);
        }

        private static object ResolveValue(string alias, bool recursive, IPublishedContent node)
        {
            return node.GetPropertyValue(alias, recursive);
        }
    }
}