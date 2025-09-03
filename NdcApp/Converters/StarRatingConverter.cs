using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace NdcApp.Converters
{
    public class StarRatingConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double rating && rating > 0)
            {
                var stars = Math.Round(rating * 2) / 2; // Round to nearest 0.5
                var fullStars = (int)Math.Floor(stars);
                var hasHalfStar = stars - fullStars >= 0.5;
                
                var starString = new string('★', fullStars);
                if (hasHalfStar)
                    starString += "☆";
                
                return $"{starString} {rating:F1}";
            }
            return "No rating";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}