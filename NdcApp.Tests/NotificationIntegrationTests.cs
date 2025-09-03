using System;
using System.Linq;
using System.Threading.Tasks;
using NdcApp.Core.Models;
using NdcApp.Core.Services;
using Xunit;

namespace NdcApp.Tests
{
    public class NotificationIntegrationTests
    {
        [Fact]
        public async Task EndToEndNotificationFlow_ShouldWork()
        {
            // Arrange
            var mockNotificationService = new MockNotificationService();
            var conferencePlanService = new ConferencePlanService();
            var talkNotificationService = new TalkNotificationService(mockNotificationService, conferencePlanService);

            // Create a sample talk
            var talk = new Talk
            {
                Day = "Monday",
                StartTime = TimeSpan.FromHours(14), // 2:00 PM
                EndTime = TimeSpan.FromHours(15),   // 3:00 PM
                Room = "Room 1",
                Title = "Introduction to .NET MAUI",
                Speaker = "John Doe",
                Category = "Mobile Development"
            };

            // Act 1: Select a talk
            conferencePlanService.SelectTalk(talk);
            await talkNotificationService.ScheduleNotificationsForSelectedTalksAsync();

            // Assert 1: Notifications should be scheduled
            Assert.Equal(3, mockNotificationService.ScheduledNotifications.Count); // 15min, 5min, 1min
            Assert.All(mockNotificationService.ScheduledNotifications, n =>
            {
                Assert.Contains("Introduction to .NET MAUI", n.Message);
                Assert.Contains("John Doe", n.Message);
                Assert.Contains("Room 1", n.Message);
            });

            // Act 2: Deselect the talk
            conferencePlanService.DeselectTalk(talk);
            await talkNotificationService.ScheduleNotificationsForSelectedTalksAsync();

            // Assert 2: All notifications should be cancelled
            Assert.Empty(mockNotificationService.ScheduledNotifications);
            Assert.True(mockNotificationService.AllNotificationsCancelled);
        }

        [Fact]
        public async Task MultipleSelectedTalks_ShouldScheduleAllNotifications()
        {
            // Arrange
            var mockNotificationService = new MockNotificationService();
            var conferencePlanService = new ConferencePlanService();
            var talkNotificationService = new TalkNotificationService(mockNotificationService, conferencePlanService);

            var talk1 = new Talk
            {
                Day = "Monday",
                StartTime = TimeSpan.FromHours(14),
                EndTime = TimeSpan.FromHours(15),
                Room = "Room 1",
                Title = "Talk 1",
                Speaker = "Speaker 1",
                Category = "Category 1"
            };

            var talk2 = new Talk
            {
                Day = "Tuesday",
                StartTime = TimeSpan.FromHours(16),
                EndTime = TimeSpan.FromHours(17),
                Room = "Room 2",
                Title = "Talk 2",
                Speaker = "Speaker 2",
                Category = "Category 2"
            };

            // Act: Select multiple talks
            conferencePlanService.SelectTalk(talk1);
            conferencePlanService.SelectTalk(talk2);
            await talkNotificationService.ScheduleNotificationsForSelectedTalksAsync();

            // Assert: Should have 6 notifications total (3 for each talk)
            Assert.Equal(6, mockNotificationService.ScheduledNotifications.Count);
            
            var talk1Notifications = mockNotificationService.ScheduledNotifications
                .Where(n => n.Message.Contains("Talk 1"))
                .ToList();
            var talk2Notifications = mockNotificationService.ScheduledNotifications
                .Where(n => n.Message.Contains("Talk 2"))
                .ToList();

            Assert.Equal(3, talk1Notifications.Count);
            Assert.Equal(3, talk2Notifications.Count);
        }

        [Fact]
        public void GetTalksStartingSoon_ShouldReturnCorrectTalks()
        {
            // Arrange
            var mockNotificationService = new MockNotificationService();
            var conferencePlanService = new ConferencePlanService();
            var talkNotificationService = new TalkNotificationService(mockNotificationService, conferencePlanService);

            var now = DateTime.Now.TimeOfDay;
            
            var soonTalk = new Talk
            {
                Day = "Monday",
                StartTime = now.Add(TimeSpan.FromMinutes(20)), // 20 minutes from now
                EndTime = now.Add(TimeSpan.FromMinutes(80)),
                Room = "Room 1",
                Title = "Soon Talk",
                Speaker = "Speaker 1",
                Category = "Category 1"
            };

            var laterTalk = new Talk
            {
                Day = "Monday",
                StartTime = now.Add(TimeSpan.FromHours(2)), // 2 hours from now
                EndTime = now.Add(TimeSpan.FromHours(3)),
                Room = "Room 2",
                Title = "Later Talk",
                Speaker = "Speaker 2",
                Category = "Category 2"
            };

            // Act: Select both talks
            conferencePlanService.SelectTalk(soonTalk);
            conferencePlanService.SelectTalk(laterTalk);

            var upcomingTalks = talkNotificationService.GetTalksStartingSoon(30); // Within 30 minutes

            // Assert: Should only return the soon talk
            Assert.Single(upcomingTalks);
            Assert.Equal("Soon Talk", upcomingTalks.First().Title);
        }
    }
}