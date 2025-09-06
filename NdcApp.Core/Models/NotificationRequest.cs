using System;

namespace NdcApp.Core.Models
{
    /// <summary>
    /// Represents a notification request for conference-related alerts.
    /// </summary>
    public class NotificationRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for this notification.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title of the notification.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message content of the notification.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the notification should be displayed.
        /// </summary>
        public DateTime ScheduledDateTime { get; set; }

        /// <summary>
        /// Gets or sets the talk associated with this notification.
        /// </summary>
        public Talk RelatedTalk { get; set; } = new();

        /// <summary>
        /// Gets or sets the type of notification.
        /// </summary>
        public NotificationType Type { get; set; }
    }

    /// <summary>
    /// Defines the types of notifications that can be scheduled.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// A reminder notification for an upcoming talk.
        /// </summary>
        TalkReminder,

        /// <summary>
        /// A notification that a talk is starting soon.
        /// </summary>
        TalkStarting,

        /// <summary>
        /// A notification about schedule updates or changes.
        /// </summary>
        ScheduleUpdate
    }
}