using Nerdery.Umbraco.Vault.Attributes;

namespace ReferenceWebsite.Models
{
    /// <summary>
    /// Enum of full month names
    /// </summary>
    public enum Month
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    /// <summary>
    /// Sample model entry showing an enum property
    /// </summary>
    [UmbracoEntity(AutoMap=true)]
    public class DateModel
    {
        [UmbracoEnumProperty]
        public Month Month { get; set; }
    }
}