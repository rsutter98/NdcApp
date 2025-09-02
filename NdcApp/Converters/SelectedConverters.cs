using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace NdcApp.Converters
{
    public class SelectedTextConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (value is bool selected && selected) ? "Selected" : "Select";
        }
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class SelectedColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (value is bool selected && selected) ? Color.FromArgb("#FFB400") : Color.FromArgb("#0A2342");
        }
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
