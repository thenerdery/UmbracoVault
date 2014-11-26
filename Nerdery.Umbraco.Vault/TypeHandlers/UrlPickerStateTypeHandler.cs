using System;
using uComponents.DataTypes.UrlPicker.Dto;

using umbraco.cms.helpers;

namespace Nerdery.Umbraco.Vault.TypeHandlers
{
    /// <summary>
    /// Used for binding Url Picker data types from Umbraco to strongly typed objects
    /// </summary>
    public class UrlPickerStateTypeHandler : ITypeHandler
    {
        private UrlPickerState Get(object value)
        {
            var urlPickerState = value as UrlPickerState;

            if (urlPickerState == null)
            {
                urlPickerState = UrlPickerState.Deserialize(value.ToString()) ?? new UrlPickerState() { NewWindow = false, Title = string.Empty, Url = string.Empty };
            }
            return urlPickerState;
        }

    	public object GetAsType<T>(object input)
    	{
    	    return Get(input);
    	}

        public Type TypeSupported
        {
            get { return typeof (UrlPickerState); }
        }
    }
}