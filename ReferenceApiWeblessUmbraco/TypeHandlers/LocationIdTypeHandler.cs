using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UmbracoVault.Attributes;
using UmbracoVault.TypeHandlers;

namespace ReferenceApiWeblessUmbraco.TypeHandlers
{
    public class LocationIdTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            var value = input.ToString();
            int result = 0;

            if (!string.IsNullOrEmpty(value))
            {
                var doc = XDocument.Parse(value);
                // ReSharper disable PossibleNullReferenceException
                int.TryParse(doc.Element("location").Element("id").Value, out result);
                // ReSharper restore PossibleNullReferenceException
            }

            return result;
        }

        public Type TypeSupported => typeof(int);
    }

    public class LocationIdUmbracoPropertyAttribute : UmbracoPropertyAttribute
    {
        public LocationIdUmbracoPropertyAttribute()
        {
            TypeHandler = new LocationIdTypeHandler();
        }
    }
}