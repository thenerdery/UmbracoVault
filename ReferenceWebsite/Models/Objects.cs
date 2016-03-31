using System;
using System.Collections.Generic;
using System.Linq;

using ReferenceWebsite.TypeHandlers;

using UmbracoVault.Attributes;

namespace ReferenceWebsite.Models
{
    /// <summary>
    ///     Representation of an image by URL and alt tag
    /// </summary>
    [UmbracoMediaEntity(AutoMap = true)]
    public class Image
    {
        [UmbracoProperty(Alias = "umbracoFile")]
        public string Url { get; set; }

        public string Alt { get; set; }
    }

    /// <summary>
    ///     Representation of a location
    /// </summary>
    [UmbracoEntity(AutoMap = true)]
    public class Location
    {
        public string Name { get; set; }

        [UmbracoJsonProperty(typeof(LatLngCoordinates))]
        public LatLngCoordinates LatLng { get; set; }
    }

    public class LatLngCoordinates
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    /// <summary>
    ///     Model of a person
    /// </summary>
    /// <remarks>
    ///     Shows how objects referencing other objects are handled by Vault.
    /// </remarks>
    [UmbracoEntity(AutoMap = true, Alias = "person")]
    public class StaffMember
    {
        public string Name { get; set; }
        public Location PrimaryLocation { get; set; }

        //This is a custom property, not a built in one. 
        //See http://bit.ly/1wuviAF in where this came from
        [LocationIdUmbracoProperty]
        public int LocationId { get; set; }
    }

    [UmbracoEntity(AutoMap = true)]
    public class ObjectsViewModel : CmsViewModelBase
    {
        public DateTime DateFromPicker { get; set; }
        public DateTime DateFromText { get; set; }
        public IEnumerable<StaffMember> MultiNodeTreePicker { get; set; }
    }
}