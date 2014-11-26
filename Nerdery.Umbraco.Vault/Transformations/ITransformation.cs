using System;

namespace Nerdery.Umbraco.Vault.Transformations
{
    public interface ITransformation
    {
        Type TypeSupported { get; }
        object Transform(object input);
    }
}
