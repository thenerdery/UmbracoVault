using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UmbracoVault.Extensions
{
    internal static class DictionaryExtensions
    {
        public static object GetOrAddThreadSafe(this IDictionary dictionary, object key, Func<object, object> valueFactory)
        {
            if (!dictionary.Contains(key))
            {
                lock (key)
                {
                    if (!dictionary.Contains(key))
                    {
                        dictionary.Add(key, valueFactory(key));
                    }
                }
            }

            return dictionary[key];
        }

        public static object GetOrAddThreadSafe(this IDictionary dictionary, object key, object value)
        {
            return dictionary.GetOrAddThreadSafe(key, k => value);
        }

        public static T GetOrAddThreadSafe<T>(this IDictionary dictionary, object key, Func<object, T> valueFactory)
        {
            var factoryWrapper = new Func<object, object>(k => valueFactory(k));
            return (T)dictionary.GetOrAddThreadSafe(key, factoryWrapper);
        }

        public static T GetOrAddThreadSafe<T>(this IDictionary dictionary, object key, T value)
        {
            return dictionary.GetOrAddThreadSafe(key, k => value);
        }
    }
}