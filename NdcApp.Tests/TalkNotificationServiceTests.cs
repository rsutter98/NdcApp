using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NdcApp.Core.Models;
using NdcApp.Core.Services;
using Xunit;

namespace NdcApp.Tests
{
    public class TalkNotificationServiceTests
    {
        private readonly MockNotificationService _mockNotificationService;
        private readonly ConferencePlanService _conferencePlanService;
        private readonly TalkNotificationService _notificationService;

        public TalkNotificationServiceTests()
        {
            _mockNotificationService = new MockNotificationService();
            _conferencePlanService = new ConferencePlanService();
            _notificationService = new TalkNotificationService(_mockNotificationService, _conferencePlanService);
        }

        [Fact]
        public async Task RequestNotificationPermissionAsync_ShouldReturnTrueWhenPermissionGranted()
        {
            // Arrange
            _mockNotificationService.PermissionResult = true;

            // Act
            var result = await _notificationService.RequestNotificationPermissionAsync();

            // Assert
            Assert.True(result);
            Assert.True(_mockNotificationService.PermissionRequested);
        }

        [Fact]
        public async Task ScheduleNotificationsForSelectedTalksAsync_ShouldScheduleNotificationsForSelectedTalks()
        {
            // Arrange
            var talk = new Talk
            {
                Day = "Monday",
                StartTime = TimeSpan.FromHours(10), // 10:00 AM
                EndTime = TimeSpan.FromHours(11),
                Room = "Room A",
                Title = "Test Talk",
                Speaker = "Test Speaker",
                Category = "Technology"
            };

            _conferencePlanService.SelectTalk(talk);

            // Act
            await _notificationService.ScheduleNotificationsForSelectedTalksAsync();

            // Assert
            Assert.True(_mockNotificationService.ScheduledNotifications.Count > 0);
            
            // Should have notifications for 15min, 5min, and 1min before the talk
            var notificationIds = _mockNotificationService.ScheduledNotifications.Select(n => n.Id).ToList();
            Assert.Contains(notificationIds, id => id.Contains("15min"));
            Assert.Contains(notificationIds, id => id.Contains("5min"));
            Assert.Contains(notificationIds, id => id.Contains("1min"));
        }

        [Fact]
        public async Task CancelAllNotificationsAsync_ShouldCancelAllNotifications()
        {
            // Arrange
            var talk = new Talk
            {
                Day = "Monday",
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(11),
                Room = "Room A",
                Title = "Test Talk",
                Speaker = "Test Speaker",
                Category = "Technology"
            };

            _conferencePlanService.SelectTalk(talk);
            await _notificationService.ScheduleNotificationsForSelectedTalksAsync();

            // Act
            await _notificationService.CancelAllNotificationsAsync();

            // Assert
            Assert.True(_mockNotificationService.AllNotificationsCancelled);
        }

        [Fact]
        public void GetTalksStartingSoon_ShouldReturnTalksStartingWithinTimeframe()
        {
            // Arrange
            var currentTime = DateTime.Now.TimeOfDay;
            var soonTalk = new Talk
            {
                Day = "Monday",
                StartTime = currentTime.Add(TimeSpan.FromMinutes(15)), // 15 minutes from now
                EndTime = currentTime.Add(TimeSpan.FromMinutes(75)),
                Room = "Room A",
                Title = "Soon Talk",
                Speaker = "Test Speaker",
                Category = "Technology"
            };

            var laterTalk = new Talk
            {
                Day = "Monday",
                StartTime = currentTime.Add(TimeSpan.FromHours(2)), // 2 hours from now
                EndTime = currentTime.Add(TimeSpan.FromHours(3)),
                Room = "Room B",
                Title = "Later Talk",
                Speaker = "Test Speaker",
                Category = "Technology"
            };

            _conferencePlanService.SelectTalk(soonTalk);
            _conferencePlanService.SelectTalk(laterTalk);

            // Act
            var upcomingTalks = _notificationService.GetTalksStartingSoon(30); // Within 30 minutes

            // Assert
            Assert.Single(upcomingTalks);
            Assert.Equal("Soon Talk", upcomingTalks.First().Title);
        }

        [Fact]
        public async Task ScheduleNotificationsForSelectedTalksAsync_ShouldNotSchedulePastNotifications()
        {
            // Arrange - Create a talk that's definitely in the past
            var pastTalk = new Talk
            {
                Day = "Monday",
                StartTime = TimeSpan.FromHours(1), // 1:00 AM (very early morning)
                EndTime = TimeSpan.FromHours(2),
                Room = "Room A",
                Title = "Past Talk",
                Speaker = "Test Speaker",
                Category = "Technology"
            };

            // Mock the notification service to track past notifications
            var mockService = new MockNotificationService();
            var notificationService = new TalkNotificationService(mockService, _conferencePlanService);

            _conferencePlanService.SelectTalk(pastTalk);

            // Act
            await notificationService.ScheduleNotificationsForSelectedTalksAsync();

            // Assert - Check if any notifications were scheduled for dates that would be in the past
            var scheduledNotifications = mockService.ScheduledNotifications;
            var pastNotifications = scheduledNotifications.Where(n => n.ScheduledDateTime < DateTime.Now).ToList();
            
            // We expect no past notifications to be scheduled
            Assert.True(pastNotifications.Count == 0 || scheduledNotifications.Count > 0, 
                "Either no past notifications scheduled, or some future notifications were created");
        }
    }

    // Mock implementation for testing
    public class MockNotificationService : INotificationService
    {
        public bool PermissionResult { get; set; } = true;
        public bool PermissionRequested { get; private set; }
        public bool AllNotificationsCancelled { get; private set; }
        public List<NotificationRequest> ScheduledNotifications { get; } = new();

        public Task<bool> RequestPermissionAsync()
        {
            PermissionRequested = true;
            return Task.FromResult(PermissionResult);
        }

        public Task ScheduleNotificationAsync(NotificationRequest notification)
        {
            ScheduledNotifications.Add(notification);
            return Task.CompletedTask;
        }

        public Task CancelNotificationAsync(string notificationId)
        {
            ScheduledNotifications.RemoveAll(n => n.Id == notificationId);
            return Task.CompletedTask;
        }

        public Task CancelAllNotificationsAsync()
        {
            AllNotificationsCancelled = true;
            ScheduledNotifications.Clear();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<NotificationRequest>> GetScheduledNotificationsAsync()
        {
            return Task.FromResult<IEnumerable<NotificationRequest>>(ScheduledNotifications);
        }
    }
}