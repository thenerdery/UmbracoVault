using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;
using Umbraco.Web;

using UmbracoVault.Exceptions;
using UmbracoVault.Extensions;

namespace UmbracoVault.TypeHandlers
{
    /// <summary>
    /// Responsible for converting Member types
    /// </summary>
    public class MemberTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            var result = typeof(T).CreateWithNoParams<T>();

            if (result == null)
                throw new ConstructorUnavailableException(typeof(T));

            var member = GetMember(input.ToString());
            if (member != null)
                Vault.Context.FillClassProperties(result, (alias, recursive) =>
                {
                    if (!member.HasProperty(alias))
                        return null;

                    return member.GetValue(alias);
                });

            return result;
        }

        private static IMember GetMember(string idString)
        {
            return UmbracoContext.Current.Application.Services.MemberService.GetById(int.Parse(idString));
        }

        public Type TypeSupported => typeof(Member);
    }
}