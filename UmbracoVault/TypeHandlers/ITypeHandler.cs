using System;
using System.Collections.Generic;
using System.Linq;

namespace UmbracoVault.TypeHandlers
{
    public interface ITypeHandler
    {
        /// <summary>
        /// Returns a strongly-typed object given a object value of it, and its expected type
        /// </summary>
        /// <typeparam name="T">The expected type to be returned</typeparam>
        /// <param name="input">The object representation of the desired value</param>
        /// <returns>A strongly-typed representation of the value</returns>
        object GetAsType<T>(object input);
        
        /// <summary>
        /// Returns the type supported by this TypeHandler
        /// </summary>
        Type TypeSupported { get; }
    }
}
