using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Nerdery.Umbraco.Vault.Attributes;
using Nerdery.Umbraco.Vault.Caching;
using Nerdery.Umbraco.Vault.Extensions;
using Nerdery.Umbraco.Vault.Transformations;
using Nerdery.Umbraco.Vault.TypeHandlers;

using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Nerdery.Umbraco.Vault
{
    /// <summary>
    /// Implementation of the IUmbracoContext
    /// </summary>
    public class UmbracoWebContext : IUmbracoContext, IUmbracoContextInternals
    {
        private readonly ClassConstructor _classConstructor;
        private readonly IList<ITransformation> _transformations;
        private readonly CacheManager _cacheManager;

        private readonly TypeHandlerFactory _typeHandlerFactory;

        public UmbracoWebContext()
        {
            //TODO: fetch classes from configuration and populate this list based on type and assembly strings
            _transformations = new List<ITransformation>
                                   {
                                       new SuperScriptTransformation()
                                   };

            _classConstructor = new ClassConstructor();
            _typeHandlerFactory = TypeHandlerFactory.Instance;
            _cacheManager = new CacheManager();
        }

        private UmbracoHelper _helper;
        protected UmbracoHelper Helper
        {
            get
            {
                if (_helper == null)
                {
                    _helper = new UmbracoHelper(UmbracoContext.Current);
                }
                return _helper;
            }
        }

        private IPublishedContent GetCurrentUmbracoContent()
        {
            if (UmbracoContext.Current.PageId != null)
            {
                return GetUmbracoContent(UmbracoContext.Current.PageId.Value);
            }
            return null;
        }

        private IPublishedContent GetUmbracoContent(int id)
        {
            var umbracoItem = Helper.TypedContent(id);
            return umbracoItem;
        }

        private IMedia GetUmbracoMedia(int id)
        {
            var mediaItem = ApplicationContext.Current.Services.MediaService.GetById(id);
            return mediaItem;
        }

        #region IUmbracoContext Members

        /// <summary>
        /// Retrieves a data item for the current node.
        /// </summary>
        /// <typeparam name="T">The object type to cast the item to</typeparam>
        /// <returns>a strongly typed version (T) of the current umbraco item.</returns>
        public T GetCurrent<T>()
        {

            var umbracoItem = GetCurrentUmbracoContent();
            if (umbracoItem == null || umbracoItem.Id <= 0)
            {
                LogHelper.Error<T>("Could not retrieve current umbraco item.", null);
                return default(T);
            }


            return GetItem<T>(umbracoItem);
        }

        /// <summary>
        /// Retrieves a data item for the current node, instantiating and hydrating a type as defined by the passed-in type parameter.
        /// This method is for use primarily when the desired return type is not known at compile time (i.e. in instances where a type is built from a string at runtime).
        /// If the desired return type is known at compile time, use GetCurrent&lt;T&gt;() instead.
        /// </summary>
        /// <param name="type">The desired Type to be returned</param>
        /// <returns>A fully-hydrated type (as defined by the Type parameter) containing data mapped from the current umbraco item</returns>
        public object GetCurrent(Type type)
        {
            var id = UmbracoContext.Current.PageId.Value;
            return GetContentById(type, id.ToString());
        }

        public object GetContentById(Type type, string idString)
        {
            var methodInfo = typeof(UmbracoWebContext).GetMethod("GetContentById", new[]{typeof(string)});
            var genericMethod = methodInfo.MakeGenericMethod(new[] { type });
            var result = genericMethod.Invoke(Vault.Context, new[] { idString });
            return result;
        }

        public T GetContentById<T>(string idString)
        {
            var id = GetIdFromString(idString);
            return GetContentById<T>(id);
        }

        public T GetMediaById<T>(string idString)
        {
            var id = GetIdFromString(idString);
            return GetMediaById<T>(id);
        }

        public T GetContentById<T>(int id)
        {
            var umbracoItem = GetUmbracoContent(id);

            if (umbracoItem == null || umbracoItem.Id <= 0)
            {
                LogHelper.Error<T>(string.Format("Could not locate umbraco item with Id of '{0}'.", id), null);
                return default(T);
            }

            return GetItem<T>(umbracoItem);
        }

        public T GetMediaById<T>(int id)
        {
            var umbracoItem = GetUmbracoMedia(id);

            if (umbracoItem == null || umbracoItem.Id <= 0)
            {
                LogHelper.Error<T>(string.Format("Could not locate umbraco media item with Id of '{0}'.", id), null);
                return default(T);
            }

            return GetMediaItem<T>(umbracoItem);
        }

        public IEnumerable<T> GetContentByCsv<T>(string csv)
        {
            return Helper.GetTypedContentByCsv(csv).Select(GetItem<T>);
        }

        public IEnumerable<T> GetByDocumentType<T>()
        {
            var items = new List<T>();
            var type = typeof(T);

            foreach (var alias in GetUmbracoEntityAliasesFromType(type))
            {
                var contents = Helper.GetContentByAlias(alias);
                items.AddRange(contents.Select(GetItem<T>));
            }

            return items;
        }

        public IEnumerable<string> GetUrlsForDocumentType<T>()
        {
            var urls = new List<string>();
            var type = typeof(T);

            foreach (var alias in GetUmbracoEntityAliasesFromType(type))
            {
                var contents = Helper.GetContentByAlias(alias);
                urls.AddRange(contents.Select(x => x.Url));
            }

            return urls;
        }

        public IEnumerable<T> GetChildren<T>(int? parentNodeId = null)
        {
            var parentNode = parentNodeId.HasValue ? GetUmbracoContent(parentNodeId.Value) : GetCurrentUmbracoContent();

            var type = typeof(T);
            var aliases = GetUmbracoEntityAliasesFromType(type);
            var nodes = parentNode.Children.Where(c => aliases.Contains(c.DocumentTypeAlias));
            return nodes.Select(GetItem<T>);
        }

        /// <summary>
        /// Given an XPath Query, it returns objects of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="query">XPath query for objects which are relative to the root</param>
        /// <returns>A list of objects that match</returns>
        public IEnumerable<T> QueryRelative<T>(string query)
        {
            var items = Helper.TypedContentAtXPath(query);
            return items.Select(GetItem<T>);
        }

        //TODO: what is the real purpose of this method? 
        ///// <summary>
        ///// Given an XPath Query, it returns an object of a specific type.
        ///// </summary>
        ///// <typeparam name="T">Type of object to return</typeparam>
        ///// <param name="query">XPath query for objects which are relative to the root</param>
        ///// <returns>A list of objects that match</returns>
        //public T QueryByUrl<T>(string query)
        //{
        //    return GetItem<T>(Helper.GetTypedContentByUrl(query));
        //}

        #endregion

        #region IUmbracoContextInternals

        /// <summary>
        /// Given a class, will return true if the class is intended to be hydrated as a Media object instead of a Content object
        /// </summary>
        /// <typeparam name="T">The type to be hydrated</typeparam>
        /// <returns>True if Media, false if Content</returns>
        public bool IsMediaRequest<T>()
        {
            var classMetaData = typeof(T).GetCustomAttributes(typeof(UmbracoMediaEntityAttribute), true).FirstOrDefault() as
                UmbracoMediaEntityAttribute;
            if (classMetaData != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Fills out class properties based on a provided, instantiated class and a Func instructing it how to get the raw property data based on alias
        /// </summary>
        /// <typeparam name="T">Type of instantiated class</typeparam>
        /// <param name="instance">An newed-up instance of T</param>
        /// <param name="getPropertyValue">A func that, provided a property alias, will return a raw value to be processed by the TypeHandler system</param>
        public void FillClassProperties<T>(T instance, Func<string, bool, object> getPropertyValue)
        {
            IEnumerable<PropertyInfo> properties;
            var optionAttribute = GetUmbracoEntityAttributes(typeof(T)).FirstOrDefault();
            if (optionAttribute != null && optionAttribute.AutoMap)
            {
                properties = GetAllPropertiesExceptOptedOut<T>();
            }
            else
            {
                properties = GetPropertiesOptedInForMapping<T>();
            }

            foreach (var propertyInfo in properties)
            {
                var propertyType = propertyInfo.PropertyType;

                //Get the attribute's property name value
                var propertyMetaData =
                    propertyInfo.GetCustomAttributes(typeof(UmbracoPropertyAttribute), true).FirstOrDefault() as
                    UmbracoPropertyAttribute;

                var recursive = GetPropertyRecursion(propertyMetaData);

                var alias = GetPropertyAlias(propertyMetaData, propertyInfo);

                //Retrieve the value -- If it's not there just ignore and move on
                var value = getPropertyValue(alias, recursive);
                if (value == null) continue;

                var transformations = _transformations.Where(x => x.TypeSupported == propertyType);
                var transformedValue = transformations.Aggregate(value,
                                                               (current, transform) => transform.Transform(current));

                if (value.GetType() == propertyType)
                {
                    propertyInfo.SetValue(instance, transformedValue, null);
                    continue;
                }

                var typeHandler = GetTypeHandler(propertyType, propertyMetaData);

                if (typeHandler == null)
                    throw new NotSupportedException(
                        string.Format("The property type {0} is not supported by Umbraco Vault.", propertyType));

                if (typeHandler is EnumTypeHandler)
                {
                    // Unfortunately, the EnumTypeHandler currently requires special attention because the GetAsType has a "where T : class" constraint.
                    // TODO: refactor this architecture so this workaround isn't necessary
                    var method = typeHandler.GetType().GetMethod("GetAsEnum");
                    var generic = method.MakeGenericMethod(propertyInfo.PropertyType);
                    var valueObject = generic.Invoke((typeHandler), new[] { transformedValue });
                    propertyInfo.SetValue(instance, valueObject, null);
                }
                else if (propertyInfo.PropertyType.IsGenericType)
                {
                    var method = typeHandler.GetType().GetMethod("GetAsType");
                    var generic = method.MakeGenericMethod(propertyInfo.PropertyType.GetGenericArguments()[0]);
                    var valueObject = generic.Invoke(typeHandler, new[] { transformedValue });
                    propertyInfo.SetValue(instance, valueObject, null);
                }
                else
                {
                    var method = typeHandler.GetType().GetMethod("GetAsType");
                    var generic = method.MakeGenericMethod(propertyInfo.PropertyType);
                    var valueObject = generic.Invoke(typeHandler, new[] { transformedValue });
                    propertyInfo.SetValue(instance, valueObject, null);
                }
            }
        }

        #endregion

        private T GetItem<T>(IPublishedContent n)
        {
            var cachedItem = _cacheManager.GetItem<T>(n.Id);
            if (cachedItem != null)
            {
                return (T)cachedItem;
            }
            var result = _classConstructor.CreateWithNode<T>(n);

            FillClassProperties(result, (alias, recursive) =>
                {
                    var value = n.GetPropertyValue(alias, recursive);
                    return value;
                });

            _cacheManager.AddItem(n.Id, result);
            return result;
        }

        private T GetMediaItem<T>(IMedia m)
        {
            var result = typeof(T).CreateWithNoParams<T>();

            FillClassProperties(result, (alias, recursive) =>
            {
                // recursive is ignored in this case
                var value = m.GetValue(alias);
                return value;
            });

            return result;
        }

        private bool GetPropertyRecursion(UmbracoPropertyAttribute umbracoPropertyBinding)
        {
            if (umbracoPropertyBinding != null)
            {
                return umbracoPropertyBinding.Recursive;
            }

            return false;
        }

        private ITypeHandler GetTypeHandler(Type propertyType, UmbracoPropertyAttribute propertyMetaData)
        {
            // TODO: this can be cleaned up... perhaps a total rethink of the architecture

            //Get the attribute's property name value
            var classMetaData = propertyType.GetCustomAttributes(typeof(UmbracoEntityAttribute), true).FirstOrDefault() as
                UmbracoEntityAttribute;

            if (classMetaData != null && classMetaData.TypeHandlerOverride != null)
            {
                // Check for type-level override
                return classMetaData.TypeHandlerOverride.CreateWithNoParams<ITypeHandler>();
            }

            if (propertyMetaData != null && propertyMetaData.TypeHandler != null)
            {
                // Check for property-level override
                return propertyMetaData.TypeHandler;
            }

            // Attempt to find default handler
            var factoryHandler = this._typeHandlerFactory.GetHandler(propertyType);

            if (factoryHandler != null)
            {
                return factoryHandler;
            }

            if (classMetaData != null && classMetaData.GetType() == typeof(UmbracoEntityAttribute))
            {
                // Finally, if no other handler has been found yet, AND the target type is an Entity
                // then let's attempt to recursively get the content
                return new UmbracoEntityTypeHandler();
            }

            // Otherwise, we got nothing
            return null;
        }

        private static string GetPropertyAlias(UmbracoPropertyAttribute umbracoPropertyBinding, PropertyInfo propertyInfo)
        {
            if(umbracoPropertyBinding != null && !string.IsNullOrWhiteSpace(umbracoPropertyBinding.Alias))
            {
                return umbracoPropertyBinding.Alias;
            }

            return string.Format("{0}{1}", propertyInfo.Name[0].ToString(CultureInfo.InvariantCulture).ToLower(),
                propertyInfo.Name.Substring(1));
        }

        /// <summary>
        /// Gets properties that are decorated with [UmbracoProperty] (opt-in mode)
        /// </summary>
        private static IEnumerable<PropertyInfo> GetPropertiesOptedInForMapping<T>()
        {
            return typeof(T).GetProperties(BindingFlags.SetProperty |
                                           BindingFlags.Public | BindingFlags.Instance)
                .Where(
                    x =>
                        x.GetCustomAttributes(typeof(UmbracoPropertyAttribute), true)
                            .Any() && x.CanWrite);
        }

        /// <summary>
        /// Gets properties that are NOT decorated with [UmbracoIgnoreProperty] (opt-out mode) 
        /// </summary>
        private static IEnumerable<PropertyInfo> GetAllPropertiesExceptOptedOut<T>()
        {
            return typeof(T).GetProperties(BindingFlags.SetProperty |
                                           BindingFlags.Public | BindingFlags.Instance)
                .Where(
                    x =>
                        !x.GetCustomAttributes(typeof(UmbracoIgnorePropertyAttribute), true)
                            .Any() && x.CanWrite
                            && x.PropertyType != typeof(IPublishedContent));
        }

        private static int GetIdFromString(string stringValue)
        {
            int result;

            int.TryParse(stringValue, out result);

            return result;
        }

        private static ReadOnlyCollection<UmbracoEntityAttribute> GetUmbracoEntityAttributes(Type type)
        {
            var result = new List<UmbracoEntityAttribute>();
            var attributes = type.GetCustomAttributes(typeof(UmbracoEntityAttribute), true) as UmbracoEntityAttribute[];
            if (attributes != null)
            {
                result.AddRange(attributes);
            }
            return result.AsReadOnly();
        }

        private static ReadOnlyCollection<string> GetUmbracoEntityAliasesFromType(Type type)
        {
            var results = new List<string>();
            var attributes = GetUmbracoEntityAttributes(type).ToList();
            if (attributes.Any())
            {
                foreach (var attribute in attributes)
                {
                    var alias = attribute.Alias;
                    if (string.IsNullOrWhiteSpace(alias))
                    {
                        alias = type.Name;
                    }
                    results.Add(alias);
                }
            }

            return results.AsReadOnly();
        }
    }
}