using System;
using System.Collections.Generic;
using System.Globalization;
using Umbraco.Core;
using UmbracoVault.Comparers;
using UmbracoVault.Extensions;

namespace UmbracoVault.Collections
{
    public class TypeAliasSet : ReadOnlySet<string>
    {
        private TypeAliasSet()
            : base(new HashSet<string>(new InvariantCultureCamelCaseStringComparer()))
        {
        }

        public static TypeAliasSet GetAliasesForUmbracoEntityType<T>()
        {
            return GetAliasesForUmbracoEntityType(typeof(T));
        }

        public static TypeAliasSet GetAliasesForUmbracoEntityType(Type type)
        {
            var set = new TypeAliasSet();
            var attributes = type.GetUmbracoEntityAttributes();

            foreach (var attribute in attributes)
            {
                var alias = attribute.Alias;
                if (string.IsNullOrWhiteSpace(alias))
                {
                    // account for doc type models use naming convention of [DocumentTypeAlias]ViewModel
                    alias = type.Name.TrimEnd("ViewModel");
                }

                set.BackingSet.Add(alias.ToCamelCase(CultureInfo.InvariantCulture));
            }


            return set;
        }

    }
}
