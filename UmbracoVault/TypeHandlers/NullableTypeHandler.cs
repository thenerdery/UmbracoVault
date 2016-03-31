using System;
using System.Collections.Generic;
using System.Linq;

namespace UmbracoVault.TypeHandlers
{
    public class NullableTypeHandler<TInnerType> : ITypeHandler where TInnerType : struct
    {
        private readonly ITypeHandler _innerHandler;

        public NullableTypeHandler(ITypeHandler innerHandler)
        {
            if (innerHandler == null)
            {
                throw new ArgumentException("Inner Handler must be set", nameof(innerHandler));
            }
            _innerHandler = innerHandler;
        }

        public object GetAsType<T>(object input)
        {
            return GetAsTypeNonNullable(input);
        }

        private object GetAsTypeNonNullable(object input)
        {
            var result = new TInnerType?((TInnerType)_innerHandler.GetAsType<TInnerType>(input));

            return result;
        }

        public Type TypeSupported => typeof(Nullable<>);
    }
}