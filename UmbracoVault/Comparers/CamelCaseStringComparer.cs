using System;
using UmbracoVault.Extensions;

namespace UmbracoVault.Comparers
{
    /// <summary>
    /// Compares two strings according to their pascal-case equivalents using an ordinal comparer
    /// </summary>
    internal class CurrentCultureCamelCaseStringComparer : CamelCaseStringComparerBase
    {
        public CurrentCultureCamelCaseStringComparer() : base(CurrentCulture)
        {
        }
    }

    /// <summary>
    /// Compares two strings according to their pascal-case equivalents using an ordinal comparer
    /// </summary>
    internal class InvariantCultureCamelCaseStringComparer : CamelCaseStringComparerBase
    {
        public InvariantCultureCamelCaseStringComparer() : base(InvariantCulture)
        {
        }
    }

    /// <summary>
    /// Compares two strings according to their pascal-case equivalents using an ordinal comparer
    /// </summary>
    internal class OrdinalCamelCaseStringComparer : CamelCaseStringComparerBase
    {
        public OrdinalCamelCaseStringComparer() : base(Ordinal)
        {
        }
    }

    internal abstract class CamelCaseStringComparerBase : StringComparer
    {
        private readonly StringComparer _comparer;

        protected CamelCaseStringComparerBase(StringComparer comparer)
        {
            _comparer = comparer;
        }

        public override int Compare(string x, string y)
        {
            return _comparer.Compare(x.ToCamelCase(), y.ToCamelCase());
        }

        public override bool Equals(string x, string y)
        {
            return _comparer.Equals(x.ToCamelCase(), y.ToCamelCase());
        }

        public override int GetHashCode(string obj)
        {
            return _comparer.GetHashCode(obj.ToCamelCase());
        }
    }
}
