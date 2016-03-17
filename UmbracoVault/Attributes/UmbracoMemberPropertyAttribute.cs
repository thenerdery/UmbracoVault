using System;
using System.Collections.Generic;
using System.Linq;

using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UmbracoMemberPropertyAttribute : UmbracoPropertyAttribute
    {
        public UmbracoMemberPropertyAttribute()
        {
            TypeHandler = new MemberTypeHandler();
        }

        /// <summary>
        /// Only needed if the camelCase name of the property is not the same as the umbraco property alias
        /// </summary>
        /// <param name="name"></param>
        public UmbracoMemberPropertyAttribute(string name)
            : base(name)
        {
            TypeHandler = new MemberTypeHandler();
        }
    }
}