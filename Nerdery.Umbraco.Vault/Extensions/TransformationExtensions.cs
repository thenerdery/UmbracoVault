using System;
using System.Collections.Generic;
using System.Linq;
using Nerdery.Umbraco.Vault.Transformations;

namespace Nerdery.Umbraco.Vault.Extensions
{
    public static class TransformationExtensions
    {
        public static IEnumerable<ITransformation> HandlesType(this IEnumerable<ITransformation> transformations, Type type)
        {
            return transformations.Where(x => x.TypeSupported == type);
        }
    }
}
