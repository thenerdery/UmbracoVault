using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using UmbracoVault.Attributes;

namespace UmbracoVault.TypeHandlers
{
    /// <summary>
    ///     Umbraco Vault Type handler for any custom json made data types
    /// </summary>
    /// <typeparam name="TObjectType">Model of custom data type</typeparam>
    [IgnoreTypeHandlerAutoRegistration]
    public class JsonDataTypeHandler<TObjectType> : ITypeHandler where TObjectType : class, new()
    {
        /// <summary>
        ///     Retrieves the property
        /// </summary>
        /// <typeparam name="T">Not used</typeparam>
        /// <param name="input">Value to be retrieved as model</param>
        /// <returns></returns>
        public object GetAsType<T>(object input)
        {
            return Get(input);
        }

        /// <summary>
        ///     The type supported by the handler
        /// </summary>
        public Type TypeSupported => typeof(TObjectType);

        /// <summary>
        ///     Converts the data as a given tyoe
        /// </summary>
        /// <param name="data">Data to be converted</param>
        /// <returns></returns>
        private static TObjectType Get(object data)
        {
            try
            {
                return data as TObjectType ?? JsonConvert.DeserializeObject<TObjectType>(data.ToString()) ?? new TObjectType();
            }
            catch
            {
                return new TObjectType();
            }
        }
    }
}