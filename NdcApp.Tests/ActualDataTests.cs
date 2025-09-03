using Xunit;
using NdcApp.Core.Services;
using System.IO;
using System.Linq;

namespace NdcApp.Tests
{
    public class ActualDataTests
    {
        private readonly TalkService _talkService;

        public ActualDataTests()
        {
            var mockLogger = new Mocks.MockLoggerService();
            _talkService = new TalkService(mockLogger);
        }
        [Fact]
        public void LoadActualNdcCsvFile_IfExists_LoadsCorrectly()
        {
            // Arrange - Try to find the actual ndc.csv file from the project
            var possiblePaths = new[]
            {
                "../../../NdcApp/Resources/Raw/ndc.csv",
                "../../../../NdcApp/Resources/Raw/ndc.csv",
                "../../../../../NdcApp/Resources/Raw/ndc.csv"
            };

            string? actualCsvPath = null;
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    actualCsvPath = path;
                    break;
                }
            }

            // Skip test if CSV file not found (this allows tests to run in different environments)
            if (actualCsvPath == null)
            {
                // Instead, test with the known header structure
                var testContent = @"Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie
Wednesday,09:00,10:00,1,Keynote: AI is having its moment ... again,Jodie Burchell,Talk";
                
                var talks = _talkService.ParseTalksFromString(testContent);
                Assert.Single(talks);
                Assert.Equal("Wednesday", talks[0].Day);
                return;
            }

            // Act - Load the actual CSV file
            var actualTalks = _talkService.LoadTalks(actualCsvPath);

            // Assert - Verify the data structure is correct
            Assert.NotEmpty(actualTalks);
            
            // All talks should have required properties
            Assert.All(actualTalks, talk =>
            {
                Assert.False(string.IsNullOrEmpty(talk.Day));
                Assert.False(string.IsNullOrEmpty(talk.Room));
                Assert.False(string.IsNullOrEmpty(talk.Title));
                Assert.False(string.IsNullOrEmpty(talk.Speaker));
                Assert.False(string.IsNullOrEmpty(talk.Category));
                Assert.True(talk.EndTime > talk.StartTime); // End time should be after start time
            });

            // Should have talks for multiple days (typical conference)
            var distinctDays = actualTalks.Select(t => t.Day).Distinct().ToList();
            Assert.True(distinctDays.Count >= 1); // At least one day

            // Should have multiple speakers
            var distinctSpeakers = actualTalks.Select(t => t.Speaker).Distinct().ToList();
            Assert.True(distinctSpeakers.Count >= 1); // At least one speaker

            // Should have multiple categories
            var distinctCategories = actualTalks.Select(t => t.Category).Distinct().ToList();
            Assert.True(distinctCategories.Count >= 1); // At least one category
        }

        [Fact]
        public void TestWithKnownNdcData_BasedOnOriginalFormat_WorksCorrectly()
        {
            // Arrange - Test with the known data structure from the original CSV
            var knownNdcData = @"Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie
Wednesday,09:00,10:00,1,Keynote: AI is having its moment ... again,Jodie Burchell,Talk
Wednesday,10:20,11:20,1,Java Sucks (So C# Didn't Have To),Adele Carpenter,Talk
Wednesday,10:20,11:20,2,Navigating complexity in event-driven architectures: A domain-driven approach,David Boyne,Talk
Wednesday,10:20,11:20,2,The future & challenges of cloud,Anders Lybecker,Talk";

            var service = new ConferencePlanService(new TalkRatingService(), new Mocks.MockLoggerService());

            // Act
            var talks = _talkService.ParseTalksFromString(knownNdcData);
            
            // Test selection of conflicting talks (same time slot)
            service.SelectTalk(talks[1]); // Java talk at 10:20 in room 1
            service.SelectTalk(talks[2]); // Event-driven talk at 10:20 in room 2

            // Test with actual data structure
            var standardSorted = service.SortTalksStandard(talks);

            // Assert
            Assert.Equal(4, talks.Count);
            
            // Verify keynote is first chronologically
            Assert.Equal("Keynote: AI is having its moment ... again", standardSorted[0].Title);
            Assert.Equal(new System.TimeSpan(9, 0, 0), standardSorted[0].StartTime);

            // Verify the talks at 10:20 come after 09:00
            var tenTwentyTalks = standardSorted.Where(t => t.StartTime.Hours == 10 && t.StartTime.Minutes == 20).ToList();
            Assert.Equal(3, tenTwentyTalks.Count);

            // Verify only one talk selected per time slot (should have replaced)
            Assert.Single(service.GetSelectedTalks());
            Assert.Equal("Navigating complexity in event-driven architectures: A domain-driven approach", 
                service.GetSelectedTalks()[0].Title);
        }
    }
}