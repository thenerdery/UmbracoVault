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

        private static object BuildProxy<T>(Type targetType, IPublishedContent node)
        {
            var t = ((object)targetType.CreateWithContentConstructor<T>(node)) ?? targetType.CreateWithNoParams<T>();

            var ops = new ProxyGenerationOptions();
            ops.AddMixinInstance(new LazyResolverMixin(node));

            // There are generic overloads for the ProxyGenerator but they would cause generic type constraings of where : class
            // to propagate all over the codebase.  The downside is all the casting being done in this class.
            return _generator.CreateInterfaceProxyWithTarget(targetType, t, ops, _interceptor);
        }

        public T CreateInstance<T>(IPublishedContent content)
        {
            return (T)BuildProxy<T>(typeof(T), content);
        }
    }
}