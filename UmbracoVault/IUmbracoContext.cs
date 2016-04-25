using System;
using System.Collections.Generic;
using System.Reflection;

namespace UmbracoVault
{
    /// <summary>
    /// Used for loading objects from Umbraco with Vault bindings
    /// </summary>
    public interface IUmbracoContext
    {
        /// <summary>
        /// Retrieves a data item for the current node.
        /// </summary>
        /// <typeparam name="T">The object type to cast the item to</typeparam>
        /// <returns>a strongly typed version (T) of the current umbraco item.</returns>
        T GetCurrent<T>() where T : class;

        /// <summary>
        /// Retrieves a data item for the current node, instantiating and hydrating a type as defined by the passed-in type parameter.
        /// This method is for use primarily when the desired return type is not known at compile time (i.e. in instances where a type is built from a string at runtime).
        /// If the desired return type is known at compile time, use GetCurrent&lt;T&gt;() instead.
        /// </summary>
        /// <param name="type">The desired Type to be returned</param>
        /// <returns>A fully-hydrated type (as defined by the Type parameter) containing data mapped from the current umbraco item</returns>
        object GetCurrent(Type type);

        /// <summary>
        /// Retrieves a data item for the provided id, instantiating and hydrating a type as defined by the passed-in type parameter.
        /// This method is for use primarily when the desired return type is not known at compile time (i.e. in instances where a type is built from a string at runtime).
        /// If the desired return type is known at compile time, use GetContentById&lt;T&gt;() instead.
        /// </summary>
        /// <param name="type">The desired Type to be returned</param>
        /// <param name="idString">The id of the Umbraco content item</param>
        /// <returns>A fully-hydrated type (as defined by the Type parameter) containing data mapped from the specified umbraco item</returns>
        object GetContentById(Type type, string idString);

        /// <summary>
        /// Retrieves a data item for the given ID
        /// </summary>
        /// <typeparam name="T">The object type to cast the item to</typeparam>
        /// <returns>a strongly typed version (T) of the requested umbraco item.</returns>
        T GetContentById<T>(int id);
        
        
        /// <summary>
        /// Retreives a data item for the given ID in a string
        /// </summary>
        /// <typeparam name="T">The object type to cast the item to</typeparam>
        /// <returns>a strongly typed version (T) of the requested umbraco item.</returns>
        T GetContentById<T>(string idString);

        /// <summary>
        /// Retrieves a media item for the given ID
        /// </summary>
        /// <typeparam name="T">The object type to cast the item to</typeparam>
        /// <returns>a strongly typed version (T) of the requested umbraco media item.</returns>
        T GetMediaById<T>(int id);


        /// <summary>
        /// Retreives a media item for the given ID in a string
        /// </summary>
        /// <typeparam name="T">The object type to cast the item to</typeparam>
        /// <returns>a strongly typed version (T) of the requested umbraco media item.</returns>
        T GetMediaById<T>(string idString);

        /// <summary>
        /// Retrieves a data item for the given string of comma separated IDs
        /// </summary>
        /// <param name="csv">A comma separated list of Node Ids</param>
        /// <typeparam name="T">The object type to cast the item to</typeparam>
        /// <returns>a strongly typed version (T) of the current umbraco item.</returns>
		IEnumerable<T> GetContentByCsv<T>(string csv);
		
        /// <summary>
        /// Creates and returns an IEnumerable of {T} as mapped to Umbraco items. 
        /// The Umbraco doc types to load are read from the [UmbracoEntity] "Alias" parameter(s) defined on {T}.
        /// All published items of the specified doc type(s) are returned.
        /// </summary>
        /// <typeparam name="T">The core type to hydrate and return, and the type that informs which Umbraco aliases to map</typeparam>
        /// <returns>An IEnumerable of {T} as mapped to Umbraco items</returns>
		IEnumerable<T> GetByDocumentType<T>();

        /// <summary>
        /// Returns a list of URLs, given a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<string> GetUrlsForDocumentType<T>();

        /// <summary>
        /// Retrieves a collection of child nodes of a specific type.
        /// </summary>
        /// <typeparam name="T">The object type to cast the item to</typeparam>
        /// <param name="parentNodeId">Optional. If omitted or null, will use the Context's CurrentNode as the parent, 
        /// otherwise the method will look up the parent by ID</param>
        /// <returns>A collection of strongly typed version (T) of children of the current node.</returns>
        IEnumerable<T> GetChildren<T>(int? parentNodeId = null);
        
        /// <summary>
        /// Given an XPath Query, it returns objects of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="query">XPath query for objects which are relative to the root</param>
        /// <returns>A list of objects that match</returns>
        IEnumerable<T> QueryRelative<T>(string query);

        ///// <summary>
        ///// Given an XPath Query, it returns objects of a specific type.
        ///// </summary>
        ///// <typeparam name="T">Type of object to return</typeparam>
        ///// <param name="query">XPath query for objects which are relative to the root</param>
        ///// <returns>A list of objects that match</returns>
        //T QueryByUrl<T>(string query) where T : class;

        /// <summary>
        /// Fills out class properties based on a provided, instantiated class and a Func instructing it how to get the raw property data based on alias.
        /// This allows the TypeHandler system to be used by related classes.
        /// </summary>
        /// <typeparam name="T">Type of instantiated class</typeparam>
        /// <param name="instance">An newed-up instance of T</param>
        /// <param name="getPropertyValue">A func that, provided a property alias and a boolean to indicate recursion, will return a raw value to be processed by the TypeHandler system</param>
        void FillClassProperties<T>(T instance, Func<string, bool, object> getPropertyValue);

        /// <summary>
        /// Fills out class properties based on a provided, instantiated class and a Func instructing it how to get the raw property data based on alias.
        /// This allows the TypeHandler system to be used by related classes.
        /// </summary>
        /// <typeparam name="T">Type of instantiated class</typeparam>
        /// <param name="instance">An newed-up instance of T</param>
        /// <param name="getPropertyValue">A func that, provided a property alias, a Property Info, and a boolean to indicate recursion, will return a raw value to be processed by the TypeHandler system</param>
        void FillClassProperties<T>(T instance, Func<string, PropertyInfo, bool, object> getPropertyValue);

        /// <summary>
        /// Uses Vault context to try to resolve value for a single property
        /// </summary>
        /// <param name="getPropertyValue">Func to return raw value for the property</param>
        /// <param name="propertyInfo">Property for which to return a value</param>
        /// <param name="value">If found value will be set</param>
        /// <returns>True if able to resolve value, false if not</returns>
        bool TryGetValueForProperty(Func<string, bool, object> getPropertyValue, PropertyInfo propertyInfo, out object value);

        /// <summary>
        /// Uses Vault context to try to resolve value for a single property
        /// </summary>
        /// <param name="getPropertyValue">Func to return raw value for the property</param>
        /// <param name="propertyInfo">Property for which to return a value</param>
        /// <param name="value">If found value will be set</param>
        /// <returns>True if able to resolve value, false if not</returns>
        bool TryGetValueForProperty(Func<string, PropertyInfo, bool, object> getPropertyValue, PropertyInfo propertyInfo, out object value);

        /// <summary>
        /// Given a class, will return true if the class is intended to be hydrated as a Media object instead of a Content object
        /// </summary>
        /// <typeparam name="T">The type to be hydrated</typeparam>
        /// <returns>True if Media, false if Content</returns>
        bool IsMediaRequest<T>();

        /// <summary>
        /// Retrieves a member for the given ID
        /// </summary>
        /// <typeparam name="T">The object type to cast the member to</typeparam>
        /// <returns>a strongly typed version (T) of the requested umbraco member.</returns>
        T GetMemberById<T>(int id);

        /// <summary>
        /// Retrieves a member for the given ID
        /// </summary>
        /// <typeparam name="T">The object type to cast the member to</typeparam>
        /// <returns>a strongly typed version (T) of the requested umbraco member.</returns>
        T GetMemberById<T>(string idString);
    }
}