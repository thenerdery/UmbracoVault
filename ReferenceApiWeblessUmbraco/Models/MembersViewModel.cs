using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using UmbracoVault.Attributes;

namespace ReferenceApiWeblessUmbraco.Models
{
    [UmbracoEntity]
    public class MembersViewModel : CmsViewModelBase
    {
        [UmbracoMemberProperty("singleMember")]
        public WebsiteUser User { get; set; }
    }

    [UmbracoMemberEntity(AutoMap = true)]
    public class WebsiteUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public IMember MemberContent { get; set; }

        public bool Approved => MemberContent.IsApproved;
    }
}