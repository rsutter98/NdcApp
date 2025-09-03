using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    public class TalkNotificationService
    {
        private readonly INotificationService _notificationService;
        private readonly ConferencePlanService _conferencePlanService;
        private readonly List<int> _reminderMinutes = new() { 15, 5, 1 }; // 15 min, 5 min, 1 min before

        public TalkNotificationService(INotificationService notificationService, ConferencePlanService conferencePlanService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _conferencePlanService = conferencePlanService ?? throw new ArgumentNullException(nameof(conferencePlanService));
        }

        public async Task<bool> RequestNotificationPermissionAsync()
        {
            return await _notificationService.RequestPermissionAsync();
        }

        public async Task ScheduleNotificationsForSelectedTalksAsync()
        {
            // Cancel existing notifications first
            await _notificationService.CancelAllNotificationsAsync();

            var selectedTalks = _conferencePlanService.GetSelectedTalks();
            var notifications = new List<NotificationRequest>();

            foreach (var talk in selectedTalks)
            {
                // Create notifications for each reminder interval
                foreach (var minutesBefore in _reminderMinutes)
                {
                    var notificationTime = GetNotificationDateTime(talk, minutesBefore);
                    
                    // Only schedule future notifications
                    if (notificationTime > DateTime.Now)
                    {
                        var notification = CreateTalkNotification(talk, minutesBefore, notificationTime);
                        notifications.Add(notification);
                    }
                }
            }

            // Schedule all notifications
            foreach (var notification in notifications)
            {
                await _notificationService.ScheduleNotificationAsync(notification);
            }
        }

        public async Task CancelAllNotificationsAsync()
        {
            await _notificationService.CancelAllNotificationsAsync();
        }

        public async Task<IEnumerable<NotificationRequest>> GetUpcomingNotificationsAsync()
        {
            return await _notificationService.GetScheduledNotificationsAsync();
        }

        private NotificationRequest CreateTalkNotification(Talk talk, int minutesBefore, DateTime notificationTime)
        {
            var notificationId = $"{talk.Day}_{talk.StartTime}_{minutesBefore}min";
            var title = minutesBefore switch
            {
                1 => "Talk starts in 1 minute!",
                5 => "Talk starts in 5 minutes",
                15 => "Talk starts in 15 minutes",
                _ => $"Talk starts in {minutesBefore} minutes"
            };

            var message = $"{talk.Title} by {talk.Speaker} in {talk.Room}";

            return new NotificationRequest
            {
                Id = notificationId,
                Title = title,
                Message = message,
                ScheduledDateTime = notificationTime,
                RelatedTalk = talk,
                Type = NotificationType.TalkReminder
            };
        }

        private DateTime GetNotificationDateTime(Talk talk, int minutesBefore)
        {
            // Parse the day to get the actual date
            var talkDate = GetTalkDate(talk.Day);
            var talkDateTime = talkDate.Add(talk.StartTime);
            return talkDateTime.AddMinutes(-minutesBefore);
        }

        private DateTime GetTalkDate(string dayName)
        {
            // For demo purposes, map days to this week
            // In a real app, you'd have actual conference dates
            var today = DateTime.Today;
            var dayOfWeek = dayName.ToLower() switch
            {
                "monday" => DayOfWeek.Monday,
                "tuesday" => DayOfWeek.Tuesday,
                "wednesday" => DayOfWeek.Wednesday,
                "thursday" => DayOfWeek.Thursday,
                "friday" => DayOfWeek.Friday,
                "saturday" => DayOfWeek.Saturday,
                "sunday" => DayOfWeek.Sunday,
                _ => DayOfWeek.Monday
            };

            // Find the next occurrence of this day
            var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
            if (daysUntilTarget == 0 && DateTime.Now.TimeOfDay > TimeSpan.FromHours(23))
            {
                daysUntilTarget = 7; // If it's late today, schedule for next week
            }
            
            return today.AddDays(daysUntilTarget == 0 ? 7 : daysUntilTarget); // If today, schedule for next week
        }

        public List<Talk> GetTalksStartingSoon(int withinMinutes = 30)
        {
            var selectedTalks = _conferencePlanService.GetSelectedTalks();
            var now = DateTime.Now.TimeOfDay;
            var soonThreshold = now.Add(TimeSpan.FromMinutes(withinMinutes));

            return selectedTalks
                .Where(talk => talk.StartTime >= now && talk.StartTime <= soonThreshold)
                .OrderBy(talk => talk.StartTime)
                .ToList();
        }
    }
}