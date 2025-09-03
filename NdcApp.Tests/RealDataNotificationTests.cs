using System;
using System.Linq;
using System.Threading.Tasks;
using NdcApp.Core.Models;
using NdcApp.Core.Services;
using Xunit;

namespace NdcApp.Tests
{
    public class RealDataNotificationTests
    {
        [Fact]
        public async Task RealConferenceTalk_ShouldGenerateCorrectNotifications()
        {
            // Arrange - Use real talk data from CSV
            var mockNotificationService = new MockNotificationService();
            var conferencePlanService = new ConferencePlanService();
            var talkNotificationService = new TalkNotificationService(mockNotificationService, conferencePlanService);

            var realTalk = new Talk
            {
                Day = "Wednesday",
                StartTime = TimeSpan.Parse("09:00"),
                EndTime = TimeSpan.Parse("10:00"),
                Room = "1",
                Title = "Keynote: AI is having its moment ... again",
                Speaker = "Jodie Burchell",
                Category = "Talk"
            };

            // Act: Select the keynote talk
            conferencePlanService.SelectTalk(realTalk);
            await talkNotificationService.ScheduleNotificationsForSelectedTalksAsync();

            // Assert: Should have 3 notifications
            Assert.Equal(3, mockNotificationService.ScheduledNotifications.Count);

            var notifications = mockNotificationService.ScheduledNotifications;

            // Verify all notifications contain the correct talk information
            Assert.All(notifications, n => 
            {
                Assert.Contains("Keynote: AI is having its moment ... again", n.Message);
                Assert.Contains("Jodie Burchell", n.Message);
                Assert.Contains("1", n.Message); // Room number
                Assert.Equal(NotificationType.TalkReminder, n.Type);
            });

            // Verify we have the right notification intervals
            var notificationTitles = notifications.Select(n => n.Title).ToList();
            Assert.Contains("Talk starts in 15 minutes", notificationTitles);
            Assert.Contains("Talk starts in 5 minutes", notificationTitles);
            Assert.Contains("Talk starts in 1 minute!", notificationTitles);
        }

        [Fact]
        public async Task ConflictingTalks_ShouldOnlyAllowOneSelection()
        {
            // Arrange - Two talks at same time slot (should conflict)
            var mockNotificationService = new MockNotificationService();
            var conferencePlanService = new ConferencePlanService();
            var talkNotificationService = new TalkNotificationService(mockNotificationService, conferencePlanService);

            var talk1 = new Talk
            {
                Day = "Wednesday",
                StartTime = TimeSpan.Parse("10:20"),
                EndTime = TimeSpan.Parse("11:20"),
                Room = "1",
                Title = "Java Sucks (So C# Didn't Have To)",
                Speaker = "Adele Carpenter",
                Category = "Talk"
            };

            var talk2 = new Talk
            {
                Day = "Wednesday",
                StartTime = TimeSpan.Parse("10:20"), // Same time slot
                EndTime = TimeSpan.Parse("11:20"),
                Room = "2",
                Title = "Navigating complexity in event-driven architectures: A domain-driven approach",
                Speaker = "David Boyne",
                Category = "Talk"
            };

            // Act: Select both talks (should overwrite first selection)
            conferencePlanService.SelectTalk(talk1);
            conferencePlanService.SelectTalk(talk2); // This should replace talk1
            await talkNotificationService.ScheduleNotificationsForSelectedTalksAsync();

            // Assert: Should only have notifications for the second talk
            Assert.Equal(3, mockNotificationService.ScheduledNotifications.Count);
            Assert.All(mockNotificationService.ScheduledNotifications, 
                n => Assert.Contains("David Boyne", n.Message));
            Assert.All(mockNotificationService.ScheduledNotifications, 
                n => Assert.DoesNotContain("Adele Carpenter", n.Message));

            // Verify the conference plan service has only one talk selected
            var selectedTalks = conferencePlanService.GetSelectedTalks();
            Assert.Single(selectedTalks);
            Assert.Equal("David Boyne", selectedTalks.First().Speaker);
        }
    }
}