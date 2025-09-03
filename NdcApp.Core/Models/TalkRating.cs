using System;

namespace NdcApp.Core.Models
{
    public class TalkRating
    {
        public string TalkId { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5 stars
        public string Comment { get; set; } = string.Empty;
        public DateTime RatingDate { get; set; } = DateTime.Now;
        public string UserId { get; set; } = "default"; // For future multi-user support
    }
}