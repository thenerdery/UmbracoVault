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

        protected override string GetAlias(IContent n)
        {
            return n.ContentType.Alias;
        }

        protected override T CreateAndHydrateItem<T>(IContent n)
        {
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

            return result;
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
    }
}
