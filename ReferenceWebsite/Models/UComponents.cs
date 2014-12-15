using System.Collections.Generic;
using Nerdery.Umbraco.Vault.Attributes;
using Nerdery.Umbraco.VaultExtensions.Attributes;
using uComponents.DataTypes.UrlPicker.Dto;

namespace ReferenceWebsite.Models
{
    /// <summary>
    /// Model containing a URLPickerState property
    /// </summary>
    [UmbracoEntity(AutoMap = true)]
    public class UComponentsUrlModel
    {
        public UrlPickerState UrlState { get; set; }
    }

    /// <summary>
    /// Row model for a uComponents DataType Grid
    /// </summary>
    [UmbracoEntity(AutoMap = true)]
    public class UComponentsDataTypeGridItem
    {
        public string Name { get; set; }
        public string Date { get; set; }
    }

    /// <summary>
    /// Model containing a DataTypeGrid
    /// </summary>
    [UmbracoEntity(AutoMap = true)]
    public class UComponentsDataTypeGridModel
    {
        [UmbracoDataTypeGridProperty]
        public List<UComponentsDataTypeGridItem> DataTypeGrid { get; set; }
    }
}