using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    /// <summary>
    /// Service interface for managing notification scheduling and permissions.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Requests permission from the user to show notifications.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if permission was granted, false otherwise.</returns>
        Task<bool> RequestPermissionAsync();

        /// <summary>
        /// Schedules a notification to be displayed at the specified time.
        /// </summary>
        /// <param name="notification">The notification request to schedule.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ScheduleNotificationAsync(NotificationRequest notification);

        /// <summary>
        /// Cancels a previously scheduled notification.
        /// </summary>
        /// <param name="notificationId">The unique identifier of the notification to cancel.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CancelNotificationAsync(string notificationId);

        /// <summary>
        /// Cancels all scheduled notifications.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CancelAllNotificationsAsync();

        /// <summary>
        /// Gets all currently scheduled notifications.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of scheduled notifications.</returns>
        Task<IEnumerable<NotificationRequest>> GetScheduledNotificationsAsync();
    }
}