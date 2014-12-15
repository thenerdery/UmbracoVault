using System;

namespace UmbracoVault.Transformations
{
    public interface ITransformation
    {
        Type TypeSupported { get; }
        object Transform(object input);
    }
}
