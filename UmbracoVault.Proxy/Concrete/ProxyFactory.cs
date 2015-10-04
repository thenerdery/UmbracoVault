using System;
using System.Collections.Generic;
using System.Linq;

using Castle.DynamicProxy;

using Umbraco.Core.Models;

using UmbracoVault.Reflection;

namespace UmbracoVault.Proxy.Concrete
{
    public class ProxyFactory : IInstanceFactory
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();
        private static readonly ProxyInterceptor _interceptor = new ProxyInterceptor();

        private static object BuildProxy<T>(IPublishedContent node)
        {
            var ops = new ProxyGenerationOptions();
            ops.AddMixinInstance(new LazyResolverMixin(node));

            return _generator.CreateClassProxy(typeof(T), ops, _interceptor);
        }

        public T CreateInstance<T>(IPublishedContent content, out bool enableFillProperties)
        {
            enableFillProperties = false;
            return (T)BuildProxy<T>(content);
        }
    }
}