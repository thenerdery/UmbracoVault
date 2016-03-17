using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

using UmbracoVault.Attributes;
using UmbracoVault.Extensions;

namespace UmbracoVault.Proxy
{
    public class ProxyInterceptor : IInterceptor
    {
        private const string GetterMethodPrefix = "get_";

        public void Intercept(IInvocation invocation)
        {
            PropertyInfo property;
            ILazyResolverMixin lazyResolverMixin;

            if (!ShouldInterceptMethod(invocation, out property) || !ShouldInterceptClass(invocation, out lazyResolverMixin))
            {
                invocation.Proceed();
                return;
            }

            var alias = PropertyAlias(invocation);
            var value = lazyResolverMixin.GetOrResolve(alias, property);
            invocation.ReturnValue = value;
        }

        private static PropertyInfo GetPropertyInfo(Type targetType, MethodInfo method)
        {
            return targetType.GetProperty(method.Name.Substring(GetterMethodPrefix.Length),
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        }

        public static string PropertyAlias(IInvocation invocation)
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

        private static bool ShouldInterceptMethod(IInvocation invocation, out PropertyInfo property)
        {
            property = GetPropertyInfo(invocation.TargetType, invocation.Method);
            if (property == null || !PropertyIsOptedIn(property) || property.GetCustomAttribute<UmbracoIgnorePropertyAttribute>() != null)
            {
                return false;
            }

            var getter = property.GetMethod;
            var isGetter = getter != null && getter.Name == invocation.Method.Name;
            var isVirtual = getter != null && getter.IsVirtual && !getter.IsFinal;

            return isGetter && isVirtual;
        }

        private static bool PropertyIsOptedIn(MemberInfo property)
        {
            var entityAttribute = property.DeclaringType.GetUmbracoEntityAttributes().FirstOrDefault();
            return entityAttribute != null && (entityAttribute.AutoMap || property.GetCustomAttribute<UmbracoPropertyAttribute>() != null);
        }
    }
}