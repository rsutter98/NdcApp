using System;

namespace NdcApp.Core.Models
{
    public class NotificationRequest
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ScheduledDateTime { get; set; }
        public Talk RelatedTalk { get; set; } = new();
        public NotificationType Type { get; set; }
    }

    public enum NotificationType
    {
        TalkReminder,
        TalkStarting,
        ScheduleUpdate
    }
}