using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using UmbracoVault.Attributes;
using UmbracoVault.Caching;
using UmbracoVault.Extensions;
using UmbracoVault.TypeHandlers;


// ReSharper disable once CheckNamespace
namespace UmbracoVault
{
    public abstract class BaseUmbracoContext : IUmbracoContext
    {
        protected readonly TypeHandlerFactory _typeHandlerFactory;
        protected readonly CacheManager _cacheManager;

        protected BaseUmbracoContext()
        {
            _typeHandlerFactory = TypeHandlerFactory.Instance;
            _cacheManager = new CacheManager();
        }

        public abstract T GetCurrent<T>() where T : class;

        public abstract object GetCurrent(Type type);

        public object GetContentById(Type type, string idString)
        {
            var methodInfo = typeof(UmbracoWebContext).GetMethod("GetContentById", new[] { typeof(string) });
            var genericMethod = methodInfo.MakeGenericMethod(type);
            var result = genericMethod.Invoke(Vault.Context, new object[] { idString });
            return result;
        }

        public abstract T GetContentById<T>(int id);

        public T GetContentById<T>(string idString)
        {
            var id = GetIdFromString(idString);
            return GetContentById<T>(id);
        }

        public T GetMediaById<T>(int id)
        {
            var umbracoItem = GetUmbracoMedia(id);

            if (umbracoItem == null || umbracoItem.Id <= 0)
            {
                LogHelper.Error<T>($"Could not locate umbraco media item with Id of '{id}'.", null);
                return default(T);
            }

            return GetMediaItem<T>(umbracoItem);
        }

        public T GetMediaById<T>(string idString)
        {
            var id = GetIdFromString(idString);
            return GetMediaById<T>(id);
        }

        public abstract IEnumerable<T> GetContentByCsv<T>(string csv);

        public abstract IEnumerable<T> GetByDocumentType<T>();

        public abstract IEnumerable<string> GetUrlsForDocumentType<T>();

        public abstract IEnumerable<T> GetChildren<T>(int? parentNodeId = null);

        public abstract T GetAncestor<T>(int? currentNodeId = null);

        public abstract IEnumerable<T> QueryRelative<T>(string query);

        /// <summary>
        /// Fills out class properties based on a provided, instantiated class and a Func instructing it how to get the raw property data based on alias
        /// </summary>
        /// <typeparam name="T">Type of instantiated class</typeparam>
        /// <param name="instance">An newed-up instance of T</param>
        /// <param name="getPropertyValue">A func that, provided a property alias, will return a raw value to be processed by the TypeHandler system</param>
        public void FillClassProperties<T>(T instance, Func<string, bool, object> getPropertyValue)
        {
            var properties = ClassConstructor.GetPropertiesToFill<T>();

            foreach (var propertyInfo in properties)
            {
                object value;
                if (TryGetValueForProperty(getPropertyValue, propertyInfo, out value))
                {
                    propertyInfo.SetValue(instance, value, null);
                }
            }
        }

        /// <summary>
        /// Fills out class properties based on a provided, instantiated class and a Func instructing it how to get the raw property data based on alias
        /// </summary>
        /// <typeparam name="T">Type of instantiated class</typeparam>
        /// <param name="instance">An newed-up instance of T</param>
        /// <param name="getPropertyValue">A func that, provided a property alias, a PropertyInfo, and a recursion indicator, will return a raw value to be processed by the TypeHandler system</param>
        public void FillClassProperties<T>(T instance, Func<string, PropertyInfo, bool, object> getPropertyValue)
        {
            var properties = ClassConstructor.GetPropertiesToFill<T>();

            foreach (var propertyInfo in properties)
            {
                object value;
                if (TryGetValueForProperty(getPropertyValue, propertyInfo, out value))
                {
                    propertyInfo.SetValue(instance, value, null);
                }
            }
        }

        public bool TryGetValueForProperty(Func<string, bool, object> getPropertyValue, PropertyInfo propertyInfo, out object value)
        {
            //Get the attribute's property name value
            var propertyMetaData = GetUmbracoPropertyAttribute(propertyInfo);
            var recursive = GetPropertyRecursion(propertyMetaData);
            var alias = GetPropertyAlias(propertyMetaData, propertyInfo);

            //Retrieve the value -- If it's not there just ignore and move on
            if (!TryGetValue(getPropertyValue, alias, recursive, out value))
            {
                return false;
            }

            value = TransformParsedValue(propertyInfo, propertyMetaData, value);

            var result = value != null;
            return result;
        }

        public bool TryGetValueForProperty(Func<string, PropertyInfo, bool, object> getPropertyValue, PropertyInfo propertyInfo, out object value)
        {
            //Get the attribute's property name value
            var propertyMetaData = GetUmbracoPropertyAttribute(propertyInfo);
            var recursive = GetPropertyRecursion(propertyMetaData);
            var alias = GetPropertyAlias(propertyMetaData, propertyInfo);

            //Retrieve the value -- If it's not there just ignore and move on
            if (!TryGetValue(getPropertyValue, alias, propertyInfo, recursive, out value))
            {
                return false;
            }

            value = TransformParsedValue(propertyInfo, propertyMetaData, value);

            var result = value != null;
            return result;
        }

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

        public T GetMemberById<T>(int id)
        {
            var umbracoItem = GetUmbracoMember(id);

            if (umbracoItem == null || umbracoItem.Id <= 0)
            {
                LogHelper.Error<T>($"Could not locate umbraco member with Id of '{id}'.", null);
                return default(T);
            }

            return GetMemberItem<T>(umbracoItem);
        }

        public T GetMemberById<T>(string idString)
        {
            var id = GetIdFromString(idString);
            return GetMemberById<T>(id);
        }

        protected static int GetIdFromString(string stringValue)
        {
            int result;

            int.TryParse(stringValue, out result);

            return result;
        }

        protected static IMedia GetUmbracoMedia(int id)
        {
            var mediaItem = ApplicationContext.Current.Services.MediaService.GetById(id);
            return mediaItem;
        }

        protected static IMember GetUmbracoMember(int id)
        {
            var member = ApplicationContext.Current.Services.MemberService.GetById(id);
            return member;
        }

        // ReSharper disable once SuggestBaseTypeForParameter - OK Here
        protected T GetMediaItem<T>(IMedia m)
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

        protected T GetMemberItem<T>(IMember m)
        {
            var result = ClassConstructor.CreateWithMember<T>(m);

            FillClassProperties(result, (alias, recursive) =>
            {
                if (!m.HasProperty(alias))
                    return null;

                var value = m.GetValue(alias);
                return value;
            });

            return result;
        }

        protected static bool TryGetValue(Func<string, bool, object> getPropertyValue, string alias, bool recursive, out object value)
        {
            try
            {
                value = getPropertyValue(alias, recursive);
            }
            catch (InvalidOperationException)
            {
                // This exception may be thrown by Umbraco 6 when attempting to read a rich text property containing macros without
                // an UmbracoContext.PageId as is the case in a SurfaceController action.
                // Opting to behave as if field is not found.
                value = null;
            }

            return value != null;
        }

        protected static bool TryGetValue(Func<string, PropertyInfo, bool, object> getPropertyValue, string alias, PropertyInfo propertyInfo, bool recursive, out object value)
        {
            try
            {
                value = getPropertyValue(alias, propertyInfo, recursive);
            }
            catch (InvalidOperationException)
            {
                // This exception may be thrown by Umbraco 6 when attempting to read a rich text property containing macros without
                // an UmbracoContext.PageId as is the case in a SurfaceController action.
                // Opting to behave as if field is not found.
                value = null;
            }

            return value != null;
        }

        protected object TransformParsedValue(PropertyInfo propertyInfo, UmbracoPropertyAttribute propertyMetaData, object parsedValue)
        {
            object value = parsedValue;

            var propertyType = propertyInfo.PropertyType;

            /*
            var transformations = _transformations.Where(x => x.TypeSupported == propertyType);
            var transformedValue = transformations.Aggregate(value,
                (current, transform) => transform.Transform(current));
            */

            var typeHandler = GetTypeHandler(propertyType, propertyMetaData);

            //unsupported if the value is not target type and there is no handler
            if (value.GetType() != propertyType && typeHandler == null)
            {
                throw new NotSupportedException($"The property type {propertyType} is not supported by Umbraco Vault.");
            }

            //if there is no handler, but value is already the target type, return value
            if (typeHandler == null)
            {
                return value;
            }

            //apply handler
            if (typeHandler is EnumTypeHandler)
            {
                // Unfortunately, the EnumTypeHandler currently requires special attention because the GetAsType has a "where T : class" constraint.
                // TODO: refactor this architecture so this workaround isn't necessary
                var method = typeHandler.GetType().GetMethod("GetAsEnum");
                var generic = method.MakeGenericMethod(propertyInfo.PropertyType);

                value = generic.Invoke(typeHandler, new[] { value });
                return value;
            }

            if (propertyInfo.PropertyType.IsGenericType)
            {
                var method = typeHandler.GetType().GetMethod("GetAsType");
                var generic = method.MakeGenericMethod(propertyInfo.PropertyType.GetGenericArguments()[0]);
                return generic.Invoke(typeHandler, new[] { value });
            }
            else
            {
                var method = typeHandler.GetType().GetMethod("GetAsType");
                var generic = method.MakeGenericMethod(propertyInfo.PropertyType);
                return generic.Invoke(typeHandler, new[] { value });
            }
        }

        private ITypeHandler GetTypeHandler(Type propertyType, UmbracoPropertyAttribute propertyMetaData)
        {
            // TODO: this can be cleaned up... perhaps a total rethink of the architecture

            //Get the attribute's property name value
            var classMetaData = propertyType.GetCustomAttributes(typeof(UmbracoEntityAttribute), true).FirstOrDefault() as
                UmbracoEntityAttribute;

            if (classMetaData?.TypeHandlerOverride != null)
            {
                // Check for type-level override
                return classMetaData.TypeHandlerOverride.CreateWithNoParams<ITypeHandler>();
            }

            if (propertyMetaData?.TypeHandler != null)
            {
                // Check for property-level override
                return propertyMetaData.TypeHandler;
            }

            // Attempt to find default handler

            // If it's generic, get the underlying type and return it as a nullable with a value, otherwise null.
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var innerType = Nullable.GetUnderlyingType(propertyType);
                var innerTypeHandler = _typeHandlerFactory.GetHandlerForType(innerType);
                if (innerTypeHandler != null)
                {
                    var nullableTypeHandlerType = typeof(NullableTypeHandler<>);
                    // ReSharper disable once PossibleNullReferenceException - Shouldn't happen (KMS 31 MAR 2016)
                    return (ITypeHandler)nullableTypeHandlerType.MakeGenericType(innerType).GetConstructor(new[] { typeof(ITypeHandler) }).Invoke(new object[] { innerTypeHandler });
                }
            }

            // Check for a default handler for this type.
            var factoryHandler = _typeHandlerFactory.GetHandlerForType(propertyType);
            if (factoryHandler != null)
            {
                return factoryHandler;
            }

            if (classMetaData?.GetType() == typeof(UmbracoEntityAttribute))
            {
                // Finally, if no other handler has been found yet, AND the target type is an Entity
                // then let's attempt to recursively get the content
                return new UmbracoEntityTypeHandler();
            }

            // Otherwise, we got nothing
            return null;
        }

        private static bool GetPropertyRecursion(UmbracoPropertyAttribute umbracoPropertyBinding)
        {
            if (umbracoPropertyBinding != null)
            {
                return umbracoPropertyBinding.Recursive;
            }

            return false;
        }

        private static UmbracoPropertyAttribute GetUmbracoPropertyAttribute(PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.GetCustomAttributes(typeof(UmbracoPropertyAttribute), true)
                                        .FirstOrDefault() as UmbracoPropertyAttribute;
            return attribute;
        }

        private static string GetPropertyAlias(UmbracoPropertyAttribute umbracoPropertyBinding, PropertyInfo propertyInfo)
        {
            if (!string.IsNullOrWhiteSpace(umbracoPropertyBinding?.Alias))
            {
                return umbracoPropertyBinding.Alias;
            }

            return $"{propertyInfo.Name[0].ToString(CultureInfo.InvariantCulture).ToLower()}{propertyInfo.Name.Substring(1)}";
        }
    }
}
