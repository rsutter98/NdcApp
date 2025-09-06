using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace NdcApp.Converters
{
    /// <summary>
    /// Converter that transforms a rating value into a visual star representation with text.
    /// </summary>
    public class StarRatingConverter : IValueConverter
    {
        private const char FullStar = '★';
        private const char HalfStar = '☆';
        private const string NoRatingText = "No rating";
        private const double HalfStarThreshold = 0.5;
        private const double RatingMultiplier = 2.0;

        /// <summary>
        /// Converts a rating value to a star representation with numeric rating.
        /// </summary>
        /// <param name="value">The rating value as a double.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture info (not used).</param>
        /// <returns>Returns a string with star symbols and numeric rating, or "No rating" for invalid input.</returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not double rating || rating <= 0)
            {
                return NoRatingText;
            }

            // Round to nearest 0.5
            var stars = Math.Round(rating * RatingMultiplier) / RatingMultiplier;
            var fullStars = (int)Math.Floor(stars);
            var hasHalfStar = stars - fullStars >= HalfStarThreshold;
            
            var starString = new string(FullStar, fullStars);
            if (hasHalfStar)
            {
                starString += HalfStar;
            }
            
            return $"{starString} {rating:F1}";
        }

        /// <summary>
        /// Not implemented - this converter is one-way only.
        /// </summary>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException("StarRatingConverter is a one-way converter.");
        }
    }
}