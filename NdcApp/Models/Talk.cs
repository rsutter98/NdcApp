using System;

namespace NdcApp.Models
{
    public class Talk
    {
        public string Day { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Room { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Speaker { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        
        // Properties for rating system
        public double AverageRating { get; set; } = 0.0;
        public int RatingCount { get; set; } = 0;
        
        // Computed property to generate unique ID
        public string Id => $"{Day}|{StartTime}|{Title}|{Speaker}";
    }
}
