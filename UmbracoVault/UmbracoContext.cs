using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;

using UmbracoVault.Extensions;
using UmbracoVault.Transformations;

namespace UmbracoVault
{
    /// <summary>
    ///     Implementation of the IUmbracoContext
    /// </summary>
    public class UmbracoWebContext : BaseUmbracoContext<IPublishedContent>
    {
        //TODO: fetch classes from configuration and populate this list based on type and assembly strings
        //TODO: Document Default Transformations
        //TODO: Re-add transformations once there's a way to opt-in to them instead of having it be global
        // ReSharper disable once CollectionNeverUpdated.Local
        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        private readonly IList<ITransformation> _transformations = new List<ITransformation>
        {
            //new SuperScriptTransformation()
        };

        private UmbracoHelper _helper;
        protected UmbracoHelper Helper => _helper ?? (_helper = new UmbracoHelper(UmbracoContext.Current));

        private IPublishedContent GetCurrentUmbracoContent()
        {
            if (UmbracoContext.Current.PageId != null)
            {
                return GetUmbracoContent(UmbracoContext.Current.PageId.Value);
            }
            return null;
        }

        /// <summary>
        ///     Retrieves a data item for the current node.
        /// </summary>
        /// <typeparam name="T">The object type to cast the item to</typeparam>
        /// <returns>a strongly typed version (T) of the current umbraco item.</returns>
        public override T GetCurrent<T>()
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
        ///     Retrieves a data item for the current node, instantiating and hydrating a type as defined by the passed-in type
        ///     parameter.
        ///     This method is for use primarily when the desired return type is not known at compile time (i.e. in instances where
        ///     a type is built from a string at runtime).
        ///     If the desired return type is known at compile time, use GetCurrent&lt;T&gt;() instead.
        /// </summary>
        /// <param name="type">The desired Type to be returned</param>
        /// <returns>A fully-hydrated type (as defined by the Type parameter) containing data mapped from the current umbraco item</returns>
        public override object GetCurrent(Type type)
        {
            // ReSharper disable once PossibleInvalidOperationException
            var id = UmbracoContext.Current.PageId.Value;
            return GetContentById(type, id.ToString());
        }

        protected override int GetId(IPublishedContent n)
        {
            return n?.Id ?? int.MinValue;
        }

        protected override string GetAlias(IPublishedContent n)
        {
            return n.DocumentTypeAlias;
        }

        protected override T CreateAndHydrateItem<T>(IPublishedContent n)
        {
            var result = ClassConstructor.CreateWithNode<T>(n);
            FillClassProperties(result, (alias, recursive) =>
            {
                var value = n.GetPropertyValue(alias, recursive);
                return value;
            });

            return result;
        }

        public override IEnumerable<T> GetContentByCsv<T>(string csv)
        {
            return Helper.GetTypedContentByCsv(csv).Select(GetItem<T>);
        }

        public override IEnumerable<T> GetByDocumentType<T>()
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

        public override IEnumerable<string> GetUrlsForDocumentType<T>()
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

        public override IEnumerable<T> GetChildren<T>(int? parentNodeId = null)
        {
            var parentNode = parentNodeId.HasValue ? GetUmbracoContent(parentNodeId.Value) : GetCurrentUmbracoContent();

            var type = typeof(T);
            var aliases = GetUmbracoEntityAliasesFromType(type);
            var nodes = parentNode.Children.Where(c => aliases.Contains(c.DocumentTypeAlias));
            return nodes.Select(GetItem<T>);
        }

        /// <summary>
        ///     Given an XPath Query, it returns objects of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="query">XPath query for objects which are relative to the root</param>
        /// <returns>A list of objects that match</returns>
        public override IEnumerable<T> QueryRelative<T>(string query)
        {
            var items = Helper.TypedContentAtXPath(query);
            return items.Select(GetItem<T>);
        }

        protected override IPublishedContent GetUmbracoContent(int id)
        {
            var umbracoItem = Helper.TypedContent(id);
            return umbracoItem;
        }

        public static ReadOnlyCollection<string> GetUmbracoEntityAliasesFromType(Type type)
        {
            var results = new HashSet<string>();
            var attributes = type.GetUmbracoEntityAttributes().ToList();
            if (attributes.Any())
            {
                foreach (var attribute in attributes)
                {
                    var alias = attribute.Alias;
                    if (string.IsNullOrWhiteSpace(alias))
                    {
                        //assumes doc type models use naming convention of [DocumentTypeAlias]ViewModel
                        alias = type.Name.TrimEnd("ViewModel");
                    }
                    results.Add(alias);
                }
            }

            return results.ToList().AsReadOnly();
        }
    }
}
