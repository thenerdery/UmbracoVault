using System;
using System.Globalization;

namespace UmbracoVault.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNumeric(this object value)
        {
            double output;

            if (value == null)
            {
                return false;
            }

            var stringValue = value.ToString();

            if (string.IsNullOrEmpty(stringValue) || string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            bool isNumeric = Double.TryParse(stringValue, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out output);

            return isNumeric;
        }
    }
}
