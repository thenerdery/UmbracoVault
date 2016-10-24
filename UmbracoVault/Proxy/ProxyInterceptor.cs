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
        private const string SetterMethodPrefix = "set_";

        public void Intercept(IInvocation invocation)
        {
            PropertyInfo property;
            ILazyResolverMixin lazyResolverMixin;

            if (!ShouldInterceptClass(invocation, out lazyResolverMixin))
            {
                invocation.Proceed();
            }
            else if (ShouldInterceptGetMethod(invocation, out property))
            {
                var alias = GetPropertyAlias(invocation);
                var value = lazyResolverMixin.GetOrResolve(alias, property);
                invocation.ReturnValue = value;
            }
            else if (ShouldInterceptSetMethod(invocation, out property))
            {
                var alias = GetPropertyAlias(invocation);
                var value = invocation.GetArgumentValue(0);

                lazyResolverMixin.Set(alias, value);
                invocation.ReturnValue = value;
            }
            else
            {
                invocation.Proceed();
            }
        }

        private static bool ShouldInterceptClass(IInvocation invocation, out ILazyResolverMixin lazyResolverMixin)
        {
            lazyResolverMixin = invocation.Proxy as ILazyResolverMixin;
            return lazyResolverMixin != null;
        }

        private static bool ShouldInterceptGetMethod(IInvocation invocation, out PropertyInfo property)
        {
            return ShouldInterceptMethod(invocation, out property, prop =>
            {
                var getter = prop.GetMethod;
                var isGetter = getter != null && getter.Name == invocation.Method.Name;
                var isVirtual = getter != null && getter.IsVirtual && !getter.IsFinal;

                return isGetter && isVirtual;
            });
        }

        private static bool ShouldInterceptSetMethod(IInvocation invocation, out PropertyInfo property)
        {
            return ShouldInterceptMethod(invocation, out property, prop =>
            {
                var setter = prop.SetMethod;
                var isSetter = setter != null && setter.Name == invocation.Method.Name;
                var isVirtual = setter != null && setter.IsVirtual && !setter.IsFinal;

                return isSetter && isVirtual;
            });
        }

        private static bool ShouldInterceptMethod(IInvocation invocation, out PropertyInfo property, Func<PropertyInfo, bool> shouldIntercept)
        {
            property = GetPropertyInfo(invocation.TargetType, invocation.Method);
            if (property == null || !PropertyIsOptedIn(property, invocation.TargetType) || property.GetCustomAttribute<UmbracoIgnorePropertyAttribute>() != null)
            {
                return false;
            }

            return shouldIntercept(property);
        }

        private static PropertyInfo GetPropertyInfo(Type targetType, MethodInfo method)
        {
            if (method.Name.StartsWith(GetterMethodPrefix, StringComparison.Ordinal))
            {
                return targetType.GetProperty(method.Name.Substring(GetterMethodPrefix.Length),
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }
            else if (method.Name.StartsWith(SetterMethodPrefix, StringComparison.Ordinal))
            {
                return targetType.GetProperty(method.Name.Substring(SetterMethodPrefix.Length),
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }

            return null;
        }

        private static bool PropertyIsOptedIn(MemberInfo property, Type topmostType)
        {
            var entityAttribute = topmostType.GetUmbracoEntityAttributes().FirstOrDefault();

            return entityAttribute != null && (entityAttribute.AutoMap || property.GetCustomAttribute<UmbracoPropertyAttribute>() != null);
        }

        public static string GetPropertyAlias(IInvocation invocation)
        {
            var umbracoProperty = invocation.Method.GetCustomAttributes(true)
                .FirstOrDefault(o => o is UmbracoPropertyAttribute) as UmbracoPropertyAttribute;

            if (umbracoProperty != null)
            {
                return umbracoProperty.Alias;
            }

            if (invocation.Method.Name.StartsWith(GetterMethodPrefix, StringComparison.Ordinal))
            {
                // Return "Foo" as property name for "get_Foo" getter method name
                return invocation.Method.Name.Substring(GetterMethodPrefix.Length);
            }
            else if (invocation.Method.Name.StartsWith(SetterMethodPrefix, StringComparison.Ordinal))
            {
                // Return "Foo" as property name for "set_Foo" getter method name
                return invocation.Method.Name.Substring(SetterMethodPrefix.Length);
            }

            return null;
        }
    }
}