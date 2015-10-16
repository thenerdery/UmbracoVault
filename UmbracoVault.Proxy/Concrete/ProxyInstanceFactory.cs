using System;
using System.Linq;

using Castle.DynamicProxy;

using Umbraco.Core.Models;

using UmbracoVault.Reflection;

namespace UmbracoVault.Proxy.Concrete
{
    public class ProxyInstanceFactory : IInstanceFactory
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();
        private static readonly ProxyInterceptor _interceptor = new ProxyInterceptor();

        private static object BuildProxy<T>(IPublishedContent node)
        {
            var ops = new ProxyGenerationOptions();
            ops.AddMixinInstance(new LazyResolverMixin(node));

            var classToProxy = typeof(T);

            // Determine whether to use constructor that takes IPublishedContent
            var useContentConstructor = classToProxy.GetConstructors().Any(c => c.GetParameters().Any(p => p.ParameterType == typeof(IPublishedContent)));

            return useContentConstructor
                ? _generator.CreateClassProxy(classToProxy, ops, new[] { node }, _interceptor)
                : _generator.CreateClassProxy(classToProxy, ops, _interceptor);
        }

        public T CreateInstance<T>(IPublishedContent content, out bool enableFillProperties)
        {
            enableFillProperties = false;
            return (T)BuildProxy<T>(content);
        }
    }
}