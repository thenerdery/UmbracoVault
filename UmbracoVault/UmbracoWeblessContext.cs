using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using UmbracoVault.Exceptions;

namespace UmbracoVault
{
    /// <summary>
    /// Implementation of the IUmbracoContext for running completely independently of the WebContext.
    /// </summary>
    public class UmbracoWeblessContext : BaseUmbracoContext
    {
        public override T GetCurrent<T>()
        {
            throw new VaultNotImplementedException();
        }

        public override object GetCurrent(Type type)
        {
            throw new VaultNotImplementedException();
        }

        public override T GetContentById<T>(int id)
        {
            var umbracoItem = GetUmbracoContent(id);

            if (umbracoItem == null || umbracoItem.Id <= 0)
            {
                LogHelper.Error<T>($"Could not locate umbraco item with Id of '{id}'.", null);
                return default(T);
            }

            return GetItem<T>(umbracoItem);
        }

        public override IEnumerable<T> GetContentByCsv<T>(string csv)
        {
            // note, this is different than the UmbracoWebContext which uses the UmbracoHelper

            var ids = csv.Split(',');
            var items = ids.Select(GetContentById<T>);
            return items;
        }

        public override IEnumerable<T> GetByDocumentType<T>()
        {
            throw new VaultNotImplementedException("The UmbracoHelper implementation relies upon XPath, which is not supported in the ContentService");
        }

        public override IEnumerable<string> GetUrlsForDocumentType<T>()
        {
            throw new VaultNotImplementedException("The UmbracoHelper relies upon XPath, which is not supported in the ContentService");
        }

        public override IEnumerable<T> GetChildren<T>(int? parentNodeId = null)
        {
            const int umbracoHomeNodeId = 1065;
            var parentId = parentNodeId.HasValue ? parentNodeId.Value : umbracoHomeNodeId;

            var children = ApplicationContext.Current.Services.ContentService
                                             .GetChildren(parentId)
                                             .Where(c => c.Published)
                                             .Select(GetItem<T>);

            return children;
        }

        public override IEnumerable<T> QueryRelative<T>(string query)
        {
            throw new VaultNotImplementedException("The Umbraco Content service does not support relative searches.");
        }

        protected IContent GetUmbracoContent(int id)
        {
            var umbracoItem = ApplicationContext.Current.Services.ContentService.GetById(id);
            return umbracoItem;
        }

        protected T GetItem<T>(IContent n)
        {
            var cachedItem = _cacheManager.GetItem<T>(n.Id);
            if (cachedItem != null)
            {
                return (T)cachedItem;
            }

            var result = ClassConstructor.CreateWithNode<T>(n);
            FillClassProperties(result, (alias, recursive) =>
            {
                var value = n.GetValue(alias);
                return value;
            });

            _cacheManager.AddItem(n.Id, result);
            return result;
        }
    }
}
