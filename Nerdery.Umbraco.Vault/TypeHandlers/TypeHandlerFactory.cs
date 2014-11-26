using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nerdery.Umbraco.Vault.Attributes;

namespace Nerdery.Umbraco.Vault.TypeHandlers
{
    /// <summary>
    /// Loads available TypeHandlers and provides a method to retrieve a handler for a given type
    /// </summary>
    internal class TypeHandlerFactory
    {
        private static TypeHandlerFactory _instance;
        private readonly Dictionary<Type, ITypeHandler> _typeHandlerDictionary;

        private TypeHandlerFactory()
        {
            //Get the type handlers for the current executing assembly that aren't flagged to Ignore
            var typeHandlers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(
                    x =>
                        x.GetInterfaces().Contains(typeof (ITypeHandler)) &&
                        x.IsClass &&
                        !x.GetCustomAttributes(typeof (IgnoreAutoLoadAttribute), true).Any())
                .Select(x =>
                {
                    var constructorInfo = x.GetConstructor(Type.EmptyTypes);
                    return constructorInfo != null ? constructorInfo.Invoke(null) : null;
                })
                .Cast<ITypeHandler>();

            _typeHandlerDictionary = new Dictionary<Type, ITypeHandler>();

            foreach (var typeHandler in typeHandlers)
            {
                _typeHandlerDictionary.Add(typeHandler.TypeSupported, typeHandler);
            }
        }

        public static TypeHandlerFactory Instance
        {
            get
            {
            	return _instance ?? (_instance = new TypeHandlerFactory());
            }
        }

        /// <summary>
        /// Retrieves a TypeHandler for a given type. Returns null if the TypeHandler does not exist
        /// </summary>
        /// <param name="t">Type for which to retrieve the TypeHandler for</param>
        /// <returns>A TypeHandler for the given type, or null if one is not available for that type.</returns>
        public ITypeHandler GetHandler(Type t)
        {
			if(t.IsGenericType)
			{
				// If it's generic, match on the type name, as the exact definition will vary based on the generic type
				return _typeHandlerDictionary.FirstOrDefault( x => x.Key.Name == t.Name).Value;
			}
            var hasType = _typeHandlerDictionary.ContainsKey(t);
            if (hasType)
            {
                return _typeHandlerDictionary[t];
            }
            return null;
        }
    }
}