using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    public interface INotificationService
    {
        Task<bool> RequestPermissionAsync();
        Task ScheduleNotificationAsync(NotificationRequest notification);
        Task CancelNotificationAsync(string notificationId);
        Task CancelAllNotificationsAsync();
        Task<IEnumerable<NotificationRequest>> GetScheduledNotificationsAsync();
    }
}