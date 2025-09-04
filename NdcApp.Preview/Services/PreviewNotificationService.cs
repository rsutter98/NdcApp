using NdcApp.Core.Models;
using NdcApp.Core.Services;

namespace NdcApp.Preview.Services
{
    /// <summary>
    /// Preview implementation of INotificationService for web-based demo.
    /// This simulates notification functionality without actual system notifications.
    /// </summary>
    public class PreviewNotificationService : INotificationService
    {
        private readonly List<NotificationRequest> _scheduledNotifications = new();
        private readonly ILoggerService _logger;

        public PreviewNotificationService(ILoggerService logger)
        {
            _logger = logger;
        }

        public Task<bool> RequestPermissionAsync()
        {
            _logger.LogInfo("Notification permission requested (Preview mode)");
            return Task.FromResult(true);
        }

        public Task ScheduleNotificationAsync(NotificationRequest request)
        {
            _scheduledNotifications.Add(request);
            _logger.LogInfo($"Notification scheduled: {request.Title} at {request.ScheduledDateTime}");
            return Task.CompletedTask;
        }

        public Task CancelNotificationAsync(string notificationId)
        {
            var notification = _scheduledNotifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                _scheduledNotifications.Remove(notification);
                _logger.LogInfo($"Notification cancelled: {notificationId}");
            }
            return Task.CompletedTask;
        }

        public Task CancelAllNotificationsAsync()
        {
            var count = _scheduledNotifications.Count;
            _scheduledNotifications.Clear();
            _logger.LogInfo($"All {count} notifications cancelled");
            return Task.CompletedTask;
        }

        public Task<IEnumerable<NotificationRequest>> GetScheduledNotificationsAsync()
        {
            return Task.FromResult<IEnumerable<NotificationRequest>>(_scheduledNotifications.ToList());
        }

        /// <summary>
        /// Gets currently scheduled notifications for preview purposes (synchronous method for local use)
        /// </summary>
        public List<NotificationRequest> GetScheduledNotifications()
        {
            return _scheduledNotifications.ToList();
        }
    }
}