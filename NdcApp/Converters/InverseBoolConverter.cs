using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace NdcApp.Converters
{
    /// <summary>
    /// Converter that inverts a boolean value (true becomes false, false becomes true).
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to its inverse.
        /// </summary>
        /// <param name="value">The boolean value to invert.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture info (not used).</param>
        /// <returns>Returns the inverted boolean value, or false for invalid input.</returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool boolValue && !boolValue;
        }

        /// <summary>
        /// Not implemented - this converter is one-way only.
        /// </summary>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => 
            throw new NotImplementedException("InverseBoolConverter is a one-way converter.");
    }
}
