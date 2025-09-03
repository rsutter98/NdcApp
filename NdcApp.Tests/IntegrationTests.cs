using Xunit;
using NdcApp.Core.Services;
using NdcApp.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NdcApp.Tests
{
    public class IntegrationTests
    {
        private readonly TalkService _talkService;

        public IntegrationTests()
        {
            var mockLogger = new Mocks.MockLoggerService();
            _talkService = new TalkService(mockLogger);
        }
        [Fact]
        public void FullWorkflow_LoadSelectSortAndPersist_WorksCorrectly()
        {
            // Arrange - Create test CSV data
            var testCsvContent = @"Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie
Wednesday,09:00,10:00,1,Keynote: AI is having its moment,Jodie Burchell,Talk
Wednesday,10:20,11:20,1,Java Sucks (So C# Didn't Have To),Adele Carpenter,Talk
Wednesday,10:20,11:20,2,Navigating complexity in event-driven architectures,David Boyne,Talk
Thursday,09:00,10:00,1,Opening Keynote,Jane Doe,Keynote
Thursday,14:00,15:00,3,Advanced C# Workshop,John Smith,Workshop";

            var service = new ConferencePlanService(new TalkRatingService(), new Mocks.MockLoggerService());

            // Act 1: Load talks from CSV content
            var talks = _talkService.ParseTalksFromString(testCsvContent);
            
            // Act 2: Select some talks
            service.SelectTalk(talks[0]); // Wednesday 09:00 Keynote
            service.SelectTalk(talks[1]); // Wednesday 10:20 Java talk
            service.SelectTalk(talks[4]); // Thursday 14:00 Workshop

            // Act 3: Sort talks in different ways
            var sortedBySpeaker = service.SortTalksBySpeaker(talks);
            var sortedByCategory = service.SortTalksByCategory(talks);
            var sortedStandard = service.SortTalksStandard(talks);

            // Act 4: Serialize and deserialize selected talks
            var serialized = service.SerializeSelectedTalks();
            var newService = new ConferencePlanService(new TalkRatingService(), new Mocks.MockLoggerService());
            newService.DeserializeSelectedTalks(serialized);

            // Assert: Verify CSV loading
            Assert.Equal(5, talks.Count);
            Assert.Equal("Keynote: AI is having its moment", talks[0].Title);
            Assert.Equal("Jodie Burchell", talks[0].Speaker);

            // Assert: Verify talk selection
            Assert.Equal(3, service.GetSelectedTalks().Count);
            Assert.True(service.IsTalkSelected(talks[0]));
            Assert.True(service.IsTalkSelected(talks[1]));
            Assert.True(service.IsTalkSelected(talks[4]));
            Assert.False(service.IsTalkSelected(talks[2])); // Not selected

            // Assert: Verify sorting by speaker
            var speakerNames = sortedBySpeaker.Select(t => t.Speaker).ToList();
            Assert.Equal("Adele Carpenter", speakerNames[0]); // A comes first
            Assert.Equal("John Smith", speakerNames[4]); // J comes last

            // Assert: Verify sorting by category
            var categories = sortedByCategory.Select(t => t.Category).ToList();
            Assert.Equal("Keynote", categories[0]); // K comes before T and W
            Assert.Equal("Talk", categories[1]); // T comes before W
            Assert.Equal("Workshop", categories[4]); // W comes last

            // Assert: Verify standard sorting (day then time)
            Assert.Equal("Wednesday", sortedStandard[0].Day); // Wednesday before Thursday
            Assert.Equal(new TimeSpan(9, 0, 0), sortedStandard[0].StartTime); // 09:00 before 10:20
            Assert.Equal("Thursday", sortedStandard[3].Day); // Thursday talks come after Wednesday

            // Assert: Verify serialization/deserialization
            Assert.Equal(3, newService.GetSelectedTalks().Count);
            var restoredTalk = newService.GetSelectedTalks().FirstOrDefault(t => t.Title.Contains("Keynote"));
            Assert.NotNull(restoredTalk);
            Assert.Equal("Wednesday", restoredTalk.Day);
            Assert.Equal(new TimeSpan(9, 0, 0), restoredTalk.StartTime);
        }

        [Fact]
        public void TimeConflictHandling_SameDayAndTime_ReplacesSelection()
        {
            // Arrange
            var service = new ConferencePlanService(new TalkRatingService(), new Mocks.MockLoggerService());
            var talk1 = new Talk
            {
                Day = "Wednesday",
                StartTime = TimeSpan.Parse("10:20"),
                EndTime = TimeSpan.Parse("11:20"),
                Room = "1",
                Title = "Java Sucks",
                Speaker = "Adele Carpenter",
                Category = "Talk"
            };
            var talk2 = new Talk
            {
                Day = "Wednesday",
                StartTime = TimeSpan.Parse("10:20"), // Same time slot
                EndTime = TimeSpan.Parse("11:20"),
                Room = "2",
                Title = "Event-driven architectures",
                Speaker = "David Boyne",
                Category = "Talk"
            };

            // Act: Select first talk, then second talk at same time
            service.SelectTalk(talk1);
            Assert.True(service.IsTalkSelected(talk1));
            Assert.Single(service.GetSelectedTalks());

            service.SelectTalk(talk2); // Should replace talk1

            // Assert: Only second talk should be selected
            Assert.False(service.IsTalkSelected(talk1));
            Assert.True(service.IsTalkSelected(talk2));
            Assert.Single(service.GetSelectedTalks());
            Assert.Equal("Event-driven architectures", service.GetSelectedTalks()[0].Title);
        }

        [Fact]
        public void EdgeCaseHandling_EmptyAndInvalidData_HandledGracefully()
        {
            // Arrange
            var service = new ConferencePlanService(new TalkRatingService(), new Mocks.MockLoggerService());

            // Test null talk handling
            service.SelectTalk(null!);
            service.DeselectTalk(null!);
            Assert.False(service.IsTalkSelected(null!));
            Assert.Empty(service.GetSelectedTalks());

            // Test empty serialization
            Assert.Equal(string.Empty, service.SerializeSelectedTalks());

            // Test invalid CSV data
            var invalidCsv = @"Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie
Invalid,Line,With,Too,Few
Wednesday,invalid_time,10:00,1,Title,Speaker,Category
,,,,,";

            var talks = _talkService.ParseTalksFromString(invalidCsv);
            Assert.Empty(talks); // Should gracefully handle all invalid lines
        }

        [Fact]
        public void ConverterFunctionality_AllConverters_WorkCorrectly()
        {
            // Arrange
            var inverseBool = new NdcApp.Core.Converters.InverseBoolConverter();
            var textConverter = new NdcApp.Core.Converters.SelectedTextConverter();
            var colorConverter = new NdcApp.Core.Converters.SelectedColorConverter();

            // Test InverseBoolConverter
            Assert.False(inverseBool.Convert(true));
            Assert.True(inverseBool.Convert(false));

            // Test SelectedTextConverter
            Assert.Equal("Selected", textConverter.Convert(true));
            Assert.Equal("Select", textConverter.Convert(false));

            // Test SelectedColorConverter
            Assert.Equal("#FFB400", colorConverter.Convert(true)); // Orange for selected
            Assert.Equal("#0A2342", colorConverter.Convert(false)); // Blue for unselected
        }

        [Fact]
        public void CompleteConferenceScenario_MultiDayEvent_WorksEndToEnd()
        {
            // Arrange - Simulate a complete conference scenario
            var conferenceData = @"Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie
Monday,09:00,10:00,Main,Welcome Keynote,CEO Speaker,Keynote
Monday,10:30,11:30,Room1,Introduction to .NET,Tech Speaker,Talk
Monday,10:30,11:30,Room2,Python Workshop,Python Expert,Workshop
Tuesday,09:00,10:00,Main,AI in Development,AI Expert,Keynote
Tuesday,14:00,16:00,Lab,Hands-on Coding,Code Mentor,Workshop
Wednesday,09:00,10:00,Main,Future of Technology,Tech Visionary,Keynote
Wednesday,15:00,16:00,Room1,Panel Discussion,Various Speakers,Panel";

            var service = new ConferencePlanService(new TalkRatingService(), new Mocks.MockLoggerService());
            var talks = _talkService.ParseTalksFromString(conferenceData);

            // Act - User selects their conference schedule
            // Select keynotes from each day
            service.SelectTalk(talks.First(t => t.Day == "Monday" && t.Category == "Keynote"));
            service.SelectTalk(talks.First(t => t.Day == "Tuesday" && t.Category == "Keynote"));
            service.SelectTalk(talks.First(t => t.Day == "Wednesday" && t.Category == "Keynote"));
            
            // Select one workshop
            service.SelectTalk(talks.First(t => t.Category == "Workshop" && t.Title.Contains("Coding")));

            // Test different sorting approaches
            var standardSort = service.SortTalksStandard(service.GetSelectedTalks());
            var speakerSort = service.SortTalksBySpeaker(service.GetSelectedTalks());

            // Assert - Verify complete scenario
            Assert.Equal(7, talks.Count); // All talks loaded
            Assert.Equal(4, service.GetSelectedTalks().Count); // 4 talks selected

            // Verify chronological order
            Assert.Equal("Monday", standardSort[0].Day);
            Assert.Equal("Wednesday", standardSort[3].Day);

            // Verify persistence works
            var serialized = service.SerializeSelectedTalks();
            Assert.Contains("Welcome Keynote", serialized);
            Assert.Contains("AI in Development", serialized);
            Assert.Contains("Future of Technology", serialized);
            Assert.Contains("Hands-on Coding", serialized);

            // Verify restoration
            var newService = new ConferencePlanService(new TalkRatingService(), new Mocks.MockLoggerService());
            newService.DeserializeSelectedTalks(serialized);
            Assert.Equal(4, newService.GetSelectedTalks().Count);
        }
    }
}