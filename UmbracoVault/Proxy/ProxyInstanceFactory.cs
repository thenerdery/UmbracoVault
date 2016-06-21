using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

using Umbraco.Core.Models;

using UmbracoVault.Extensions;
using UmbracoVault.Reflection;

namespace UmbracoVault.Proxy
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
                ? _generator.CreateClassProxy(classToProxy, ops, new object[] { node }, _interceptor)
                : _generator.CreateClassProxy(classToProxy, ops, _interceptor);
        }

        private static object BuildProxy<T>(IContent node)
        {
            var ops = new ProxyGenerationOptions();
            ops.AddMixinInstance(new LazyContentResolverMixin(node));

            var classToProxy = typeof(T);

            // Determine whether to use constructor that takes IPublishedContent
            var useContentConstructor = classToProxy.GetConstructors().Any(c => c.GetParameters().Any(p => p.ParameterType == typeof(IContent)));

            return useContentConstructor
                ? _generator.CreateClassProxy(classToProxy, ops, new object[] { node }, _interceptor)
                : _generator.CreateClassProxy(classToProxy, ops, _interceptor);
        }

        public T CreateInstance<T>(IPublishedContent content)
        {
            return (T)BuildProxy<T>(content);
        }

        public T CreateInstance<T>(IContent content)
        {
            return (T)BuildProxy<T>(content);
        }

        public IList<PropertyInfo> GetPropertiesToFill<T>()
        {
            return GetPropertiesToFill(typeof(T));
        }

        public IList<PropertyInfo> GetPropertiesToFill(Type type)
        {
            // Interface properties will be virtual and also final without having a virtual keyword on them.
            // We don't want to return properties that are virtual but NOT final because
            // these will be properties that are actually flagged as virtual, so the proxy can handle it
            // http://stackoverflow.com/a/17298167/53001
            return type.GetDefaultPropertiesToFill().Where(p => 
                                                            p.GetMethod != null 
                                                            && (!p.GetMethod.IsVirtual || 
                                                                (p.GetMethod.IsVirtual && p.GetMethod.IsFinal))).ToList();
        }
    }
}