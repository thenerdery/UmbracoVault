using System;
using System.Collections.Generic;
using System.Linq;

using Castle.DynamicProxy;

using Umbraco.Core.Models;

using UmbracoVault.Extensions;
using UmbracoVault.Reflection;

namespace UmbracoVault.Proxy.Concrete
{
    public class ProxyFactory : IInstanceFactory
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();
        private static readonly ProxyInterceptor _interceptor = new ProxyInterceptor();

        /// <summary>
        ///     Creates a proxy object T using the provided content node for data
        /// </summary>
        /// <typeparam name="T">Model to proxy</typeparam>
        /// <param name="node">Data used to back virtual properties on the model</param>
        /// <returns>Dynamic proxy for provided type</returns>
        private static T BuildProxy<T>(IPublishedContent node) where T : class
        {
            var targetType = typeof(T);
            var t = targetType.CreateWithContentConstructor<T>(node) ?? targetType.CreateWithNoParams<T>();

            var ops = new ProxyGenerationOptions();
            ops.AddMixinInstance(new LazyResolverMixin(node));

            return (T)_generator.CreateInterfaceProxyWithTarget(targetType, t, ops, _interceptor);
        }

        public T CreateInstance<T>(IPublishedContent content) where T : class
        {
            return BuildProxy<T>(content);
        }
    }
}