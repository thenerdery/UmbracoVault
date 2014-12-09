using Nerdery.Umbraco.Vault.Attributes;

namespace ReferenceWebsite.Models
{
    /// <summary>
    /// Representation of an image by URL and alt tag
    /// </summary>
    [UmbracoMediaEntity(AutoMap = true)]
    public class Image
    {
        [UmbracoProperty(Alias = "umbracoFile")]
        public string Url { get; set; }
        public string Alt { get; set; }
    }

    /// <summary>
    /// Representation of a location
    /// </summary>
    [UmbracoEntity(AutoMap = true)]
    public class Location
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Model of a person
    /// </summary>
    /// <remarks>
    /// Shows how objects referencing other objects are handled by Vault.
    /// </remarks>
    [UmbracoEntity(AutoMap = true, Alias = "person")]
    public class StaffMember
    {
        public string Name { get; set; }
        public Location PrimaryLocation { get; set; }
    }
}