using System;

namespace Nerdery.Umbraco.Vault
{
    internal interface IUmbracoContextInternals
    {
        /// <summary>
        /// Fills out class properties based on a provided, instantiated class and a Func instructing it how to get the raw property data based on alias.
        /// This allows the TypeHandler system to be used by related classes.
        /// </summary>
        /// <typeparam name="T">Type of instantiated class</typeparam>
        /// <param name="instance">An newed-up instance of T</param>
        /// <param name="getPropertyValue">A func that, provided a property alias and a boolean to indicate recursion, will return a raw value to be processed by the TypeHandler system</param>
        void FillClassProperties<T>(T instance, Func<string, bool, object> getPropertyValue);

        /// <summary>
        /// Given a class, will return true if the class is intended to be hydrated as a Media object instead of a Content object
        /// </summary>
        /// <typeparam name="T">The type to be hydrated</typeparam>
        /// <returns>True if Media, false if Content</returns>
        bool IsMediaRequest<T>();
    }
}
