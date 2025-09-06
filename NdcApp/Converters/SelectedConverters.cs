using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace NdcApp.Converters
{
    /// <summary>
    /// Converter that transforms a boolean value into text indicating selection status.
    /// </summary>
    public class SelectedTextConverter : IValueConverter
    {
        private const string SelectedText = "Selected";
        private const string NotSelectedText = "Select";

        /// <summary>
        /// Converts a boolean value to appropriate selection text.
        /// </summary>
        /// <param name="value">The boolean value indicating selection status.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture info (not used).</param>
        /// <returns>Returns "Selected" if true, "Select" if false or invalid input.</returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool selected && selected ? SelectedText : NotSelectedText;
        }

        /// <summary>
        /// Not implemented - this converter is one-way only.
        /// </summary>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => 
            throw new NotImplementedException("SelectedTextConverter is a one-way converter.");
    }

    /// <summary>
    /// Converter that transforms a boolean value into appropriate colors for selection status.
    /// </summary>
    public class SelectedColorConverter : IValueConverter
    {
        private static readonly Color SelectedColor = Color.FromArgb("#FFB400");
        private static readonly Color NotSelectedColor = Color.FromArgb("#0A2342");

        /// <summary>
        /// Converts a boolean value to appropriate selection color.
        /// </summary>
        /// <param name="value">The boolean value indicating selection status.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture info (not used).</param>
        /// <returns>Returns orange color if selected, dark blue if not selected or invalid input.</returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool selected && selected ? SelectedColor : NotSelectedColor;
        }

        /// <summary>
        /// Not implemented - this converter is one-way only.
        /// </summary>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => 
            throw new NotImplementedException("SelectedColorConverter is a one-way converter.");
    }
}
