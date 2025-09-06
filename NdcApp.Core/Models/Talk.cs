using System;

namespace NdcApp.Core.Models
{
    /// <summary>
    /// Represents a conference talk with all relevant information including timing, speaker, and rating data.
    /// </summary>
    public class Talk
    {
        /// <summary>
        /// Gets or sets the day of the week when the talk occurs.
        /// </summary>
        public string Day { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time of the talk.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the talk.
        /// </summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Gets or sets the room where the talk takes place.
        /// </summary>
        public string Room { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title of the talk.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the speaker presenting the talk.
        /// </summary>
        public string Speaker { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category or topic area of the talk.
        /// </summary>
        public string Category { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the average rating for this talk based on user ratings.
        /// </summary>
        public double AverageRating { get; set; } = 0.0;

        /// <summary>
        /// Gets or sets the total number of ratings received for this talk.
        /// </summary>
        public int RatingCount { get; set; } = 0;
        
        /// <summary>
        /// Gets a unique identifier for this talk based on day, start time, title, and speaker.
        /// </summary>
        public string Id => $"{Day}|{StartTime}|{Title}|{Speaker}";
    }
}