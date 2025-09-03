using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NdcApp.Core.Models;
using NdcApp.Core.Services;
using Plugin.LocalNotification;

namespace NdcApp.Services
{
    public class LocalNotificationService : NdcApp.Core.Services.INotificationService
    {
        private readonly List<NdcApp.Core.Models.NotificationRequest> _scheduledNotifications = new();

        public async Task<bool> RequestPermissionAsync()
        {
            try
            {
                return await LocalNotificationCenter.Current.RequestNotificationPermission();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task ScheduleNotificationAsync(NdcApp.Core.Models.NotificationRequest notification)
        {
            try
            {
                var request = new Plugin.LocalNotification.NotificationRequest
                {
                    NotificationId = GetHashCode(notification.Id),
                    Title = notification.Title,
                    Description = notification.Message,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = notification.ScheduledDateTime
                    }
                };

                await LocalNotificationCenter.Current.Show(request);
                
                // Track scheduled notification
                _scheduledNotifications.Add(notification);
            }
            catch (Exception)
            {
                // Log error in production code
                throw;
            }
        }

        public async Task CancelNotificationAsync(string notificationId)
        {
            try
            {
                var id = GetHashCode(notificationId);
                LocalNotificationCenter.Current.Cancel(id);
                
                // Remove from tracking
                _scheduledNotifications.RemoveAll(n => n.Id == notificationId);
                
                await Task.CompletedTask;
            }
            catch (Exception)
            {
                // Log error in production code
                throw;
            }
        }

        public async Task CancelAllNotificationsAsync()
        {
            try
            {
                LocalNotificationCenter.Current.CancelAll();
                _scheduledNotifications.Clear();
                
                await Task.CompletedTask;
            }
            catch (Exception)
            {
                // Log error in production code
                throw;
            }
        }

        public Task<IEnumerable<NdcApp.Core.Models.NotificationRequest>> GetScheduledNotificationsAsync()
        {
            // Filter out past notifications
            var now = DateTime.Now;
            var upcomingNotifications = _scheduledNotifications
                .Where(n => n.ScheduledDateTime > now)
                .AsEnumerable();
            
            return Task.FromResult(upcomingNotifications);
        }

        private static int GetHashCode(string input)
        {
            return Math.Abs(input.GetHashCode());
        }
    }
}