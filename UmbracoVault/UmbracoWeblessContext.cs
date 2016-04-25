using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using UmbracoVault.Exceptions;
using UmbracoVault.Extensions;

namespace UmbracoVault
{
    /// <summary>
    /// Implementation of the IUmbracoContext for running completely independently of the WebContext.
    /// </summary>
    public class UmbracoWeblessContext : BaseUmbracoContext<IContent>
    {
        public override T GetCurrent<T>()
        {
            throw new VaultNotImplementedException("Since we aren't running under the Umbraco Web Context, there is no 'current' in this context");
        }

        public override object GetCurrent(Type type)
        {
            throw new VaultNotImplementedException("Since we aren't running under the Umbraco Web Context, there is no 'current' in this context");
        }

        protected override int GetId(IContent n)
        {
            return n?.Id ?? int.MinValue;
        }

        public override IEnumerable<T> GetContentByCsv<T>(string csv)
        {
            // note, this is different than the UmbracoWebContext implementation which uses the UmbracoHelper
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
            throw new VaultNotImplementedException("The Umbraco Content service does not support relative searches due to lack of XPath support.");
        }

        protected override IContent GetUmbracoContent(int id)
        {
            var umbracoItem = ApplicationContext.Current.Services.ContentService.GetById(id);
            return umbracoItem;
        }

        protected override T GetItem<T>(IContent n)
        {
            var typesMetaData = this.VaultEntities.FirstOrDefault(x => x.Type == typeof(T));
            var explicitType = this.VaultEntities.FirstOrDefault(x =>
                                    typeof(T).IsAssignableFrom(x.Type)
                                        && (x.Type.Name.Equals(n.ContentType.Alias, StringComparison.CurrentCultureIgnoreCase)
                                            || (x.MetaData.Alias != null && x.MetaData.Alias.Equals(n.ContentType.Alias, StringComparison.CurrentCultureIgnoreCase))));

            var useExplicitType = explicitType != null && typesMetaData != null && typesMetaData.MetaData.ReturnStronglyTypedChildren;
            var typeToUse = useExplicitType ? explicitType.Type : typeof(T);

            var getItemMethod = this.GetType()
                                    .GetMethod(nameof(GetItemForExplicitType), BindingFlags.Instance | BindingFlags.NonPublic)
                                    .MakeGenericMethod(typeToUse);

            var result = getItemMethod.Invoke(this, new object[] { n });
            return (T)result;
        }

        private T GetItemForExplicitType<T>(IContent n)
        {
            var cachedItem = _cacheManager.GetItem<T>(n.Id);
            if (cachedItem != null)
            {
                return (T)cachedItem;
            }

            var result = ClassConstructor.CreateWithContent<T>(n);
            FillClassProperties(result, (alias, propertyInfo, recursive) =>
            {
                var targetType = propertyInfo.PropertyType;

                var containsProperty = n.HasProperty(alias);

                if (containsProperty)
                {
                    var property = n.Properties.First(p => string.Equals(p.Alias, alias, StringComparison.InvariantCultureIgnoreCase));

                    // the IPublishedNode.GetPropertyValue() will fetch the prevalue of the numeric item if 
                    // the target is a string.  This emulates that functionality.
                    if (targetType == typeof(string) && property.Value.IsNumeric())
                    {
                        var name = n.GetPrevalues(ApplicationContext.Current.Services.DataTypeService, alias);
                        return name;
                    }

                    return property.Value;
                }
                else
                {
                    if (string.Equals("name", propertyInfo.Name, StringComparison.CurrentCultureIgnoreCase) && propertyInfo.PropertyType == typeof(string))
                    {
                        return n.Name;
                    }
                }

                return null;
            });

            _cacheManager.AddItem(n.Id, result);
            return result;
        }
    }
}
