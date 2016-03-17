using System;
using System.Collections.Generic;
using System.Linq;

using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UmbracoMemberEntityAttribute : UmbracoEntityAttribute
    {
        public override Type TypeHandlerOverride
        {
            get { return typeof(MemberTypeHandler); }
            set
            {
                throw new NotSupportedException("Cannot override type for UmbracoMember type handler");
            }
        }
    }
}