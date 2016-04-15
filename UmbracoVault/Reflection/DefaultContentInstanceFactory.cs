using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace UmbracoVault.Reflection
{
    public class DefaultContentInstanceFactory : IContentInstanceFactory
    {
        public T CreateInstance<T>(IContent content)
        {
            throw new NotImplementedException();
        }

        public IList<PropertyInfo> GetPropertiesToFill<T>()
        {
            throw new NotImplementedException();
        }

        public IList<PropertyInfo> GetPropertiesToFill(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
