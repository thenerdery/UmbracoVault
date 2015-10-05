using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

using UmbracoVault.Attributes;

namespace UmbracoVault.Proxy.Concrete
{
    public class ProxyInterceptor : IInterceptor
    {
        private const string GetterMethodPrefix = "get_";

        public void Intercept(IInvocation invocation)
        {
            ILazyResolverMixin lazyResolverMixin;

            if (!ShouldInterceptMethod(invocation) ||
                !ShouldInterceptClass(invocation, out lazyResolverMixin))
            {
                invocation.Proceed();
                return;
            }

            var alias = PropertyAlias(invocation);
            var property = GetPropertyInfo(invocation.TargetType, invocation.Method);

            var value = lazyResolverMixin.GetOrResolve(alias, property);
            invocation.ReturnValue = value;
        }

        private PropertyInfo GetPropertyInfo(Type targetType, MethodInfo method)
        {
            return targetType.GetProperty(method.Name.Substring(GetterMethodPrefix.Length),
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        }

        private string PropertyAlias(IInvocation invocation)
        {
            var umbracoProperty = invocation.Method.GetCustomAttributes(true)
                .FirstOrDefault(o => o is UmbracoPropertyAttribute) as UmbracoPropertyAttribute;

            if (umbracoProperty != null)
            {
                return umbracoProperty.Alias;
            }

            // Return "Foo" as property name for "get_Foo" getter method name
            return invocation.Method.Name.Substring(GetterMethodPrefix.Length);
        }

        private static bool ShouldInterceptClass(IInvocation invocation, out ILazyResolverMixin lazyResolverMixin)
        {
            lazyResolverMixin = invocation.Proxy as ILazyResolverMixin;
            return lazyResolverMixin != null;
        }

        private static bool ShouldInterceptMethod(IInvocation invocation)
        {
            // Only intercepting property getters
            return invocation.Method.IsSpecialName &&
                   invocation.Method.Name.StartsWith(GetterMethodPrefix, StringComparison.Ordinal);
        }
    }
}