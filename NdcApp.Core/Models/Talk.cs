using System;

namespace NdcApp.Core.Models
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
    }
}