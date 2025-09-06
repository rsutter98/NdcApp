using System;

namespace NdcApp.Core.Models
{
    /// <summary>
    /// Represents a user rating for a conference talk.
    /// </summary>
    public class TalkRating
    {
        /// <summary>
        /// Gets or sets the unique identifier of the talk being rated.
        /// </summary>
        public string TalkId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rating value from 1 to 5 stars.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets an optional comment about the talk.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the rating was given.
        /// </summary>
        public DateTime RatingDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the identifier of the user who provided the rating.
        /// </summary>
        public string UserId { get; set; } = "default";
    }
}