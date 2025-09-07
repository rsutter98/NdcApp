using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using NdcApp.Core.Models;
using NdcApp.Core.Services;
using NdcApp.Tests.Mocks;

namespace NdcApp.Tests
{
    public class PerformanceTests
    {
        private readonly ITalkService _talkService;
        private readonly ITalkFilterService _talkFilterService;
        private readonly IConferencePlanService _conferencePlanService;

        public PerformanceTests()
        {
            var mockLogger = new MockLoggerService();
            var mockRatingService = new TalkRatingService();
            _talkService = new TalkService(mockLogger);
            _talkFilterService = new TalkFilterService();
            _conferencePlanService = new ConferencePlanService(mockRatingService, mockLogger);
        }

        [Fact]
        public void LoadTalks_Performance_ShouldCompleteQuickly()
        {
            // Arrange - Create test CSV data with realistic size
            var testData = CreateTestCsvData(1000); // Test with 1000 talks
            var stopwatch = Stopwatch.StartNew();

            // Act
            var talks = _talkService.ParseTalksFromString(testData);
            stopwatch.Stop();

            // Assert
            Assert.NotEmpty(talks);
            Assert.True(stopwatch.ElapsedMilliseconds < 100, $"LoadTalks took {stopwatch.ElapsedMilliseconds}ms, expected < 100ms");
        }

        [Fact]
        public void FilterTalks_Performance_ShouldCompleteQuickly()
        {
            // Arrange - Create test data
            var talks = CreateTestTalks(1000);
            var stopwatch = Stopwatch.StartNew();

            // Act
            var filtered = _talkFilterService.FilterTalks(talks, "test");
            stopwatch.Stop();

            // Assert
            Assert.NotNull(filtered);
            Assert.True(stopwatch.ElapsedMilliseconds < 50, $"FilterTalks took {stopwatch.ElapsedMilliseconds}ms, expected < 50ms");
        }

        [Fact]
        public void RefreshTalksView_Simulation_Performance()
        {
            // Arrange - Simulate the RefreshTalksView logic
            var allTalks = CreateTestTalks(1000);
            var selectedTalks = new Dictionary<string, Talk>();
            
            // Select some talks
            for (int i = 0; i < 100; i++)
            {
                var talk = allTalks[i];
                var key = $"{talk.Day}|{talk.StartTime}";
                selectedTalks[key] = talk;
            }

            var stopwatch = Stopwatch.StartNew();

            // Act - Simulate current RefreshTalksView logic
            var filteredTalks = _talkFilterService.FilterTalks(allTalks, "");
            
            var grouped = filteredTalks
                .GroupBy(t => new { t.Day, t.StartTime })
                .OrderBy(g => g.Key.Day)
                .ThenBy(g => g.Key.StartTime)
                .ToList();

            var displayList = new List<TalkDisplayItem>();
            foreach (var group in grouped)
            {
                foreach (var talk in group)
                {
                    if (talk == null) continue;
                    var key = $"{talk.Day}|{talk.StartTime}";
                    bool isSelected = selectedTalks.ContainsKey(key) && selectedTalks[key].Title == talk.Title;
                    displayList.Add(new TalkDisplayItem
                    {
                        Talk = talk ?? new Talk(),
                        IsSelected = isSelected
                    });
                }
            }
            
            stopwatch.Stop();

            // Assert
            Assert.NotEmpty(displayList);
            // Current logic should complete within reasonable time, but we'll set a generous limit
            Assert.True(stopwatch.ElapsedMilliseconds < 200, $"RefreshTalksView simulation took {stopwatch.ElapsedMilliseconds}ms, expected < 200ms");
        }

        [Fact]
        public void OptimizedRefreshTalksView_Simulation_Performance()
        {
            // Arrange - Simulate optimized RefreshTalksView logic
            var allTalks = CreateTestTalks(1000);
            var selectedTalksSet = new HashSet<string>();
            
            // Select some talks using optimized structure
            for (int i = 0; i < 100; i++)
            {
                var talk = allTalks[i];
                var key = $"{talk.Day}|{talk.StartTime}|{talk.Title}";
                selectedTalksSet.Add(key);
            }

            var stopwatch = Stopwatch.StartNew();

            // Act - Simulate optimized RefreshTalksView logic
            var filteredTalks = _talkFilterService.FilterTalks(allTalks, "");
            
            var displayList = filteredTalks
                .OrderBy(t => t.Day)
                .ThenBy(t => t.StartTime)
                .Select(talk => new TalkDisplayItem
                {
                    Talk = talk,
                    IsSelected = selectedTalksSet.Contains($"{talk.Day}|{talk.StartTime}|{talk.Title}")
                })
                .ToList();
            
            stopwatch.Stop();

            // Assert
            Assert.NotEmpty(displayList);
            // Optimized logic should be significantly faster
            Assert.True(stopwatch.ElapsedMilliseconds < 50, $"Optimized RefreshTalksView simulation took {stopwatch.ElapsedMilliseconds}ms, expected < 50ms");
        }

        private string CreateTestCsvData(int count)
        {
            var header = "Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie\n";
            var lines = new List<string> { header.TrimEnd() };
            
            var days = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            var categories = new[] { "Talk", "Workshop", "Panel", "Keynote" };
            var rooms = new[] { "1", "2", "3", "4", "5" };

            for (int i = 0; i < count; i++)
            {
                var day = days[i % days.Length];
                var hour = 9 + (i % 8); // 9-16 hours
                var startTime = $"{hour:D2}:00";
                var endTime = $"{hour + 1:D2}:00";
                var room = rooms[i % rooms.Length];
                var title = $"Test Talk {i}";
                var speaker = $"Speaker {i % 20}"; // 20 different speakers
                var category = categories[i % categories.Length];
                
                lines.Add($"{day},{startTime},{endTime},{room},{title},{speaker},{category}");
            }
            
            return string.Join("\n", lines);
        }

        private List<Talk> CreateTestTalks(int count)
        {
            var talks = new List<Talk>();
            var days = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            var categories = new[] { "Talk", "Workshop", "Panel", "Keynote" };
            var rooms = new[] { "1", "2", "3", "4", "5" };

            for (int i = 0; i < count; i++)
            {
                var day = days[i % days.Length];
                var hour = 9 + (i % 8); // 9-16 hours
                var startTime = new TimeSpan(hour, 0, 0);
                var endTime = new TimeSpan(hour + 1, 0, 0);
                var room = rooms[i % rooms.Length];
                var title = $"Test Talk {i}";
                var speaker = $"Speaker {i % 20}"; // 20 different speakers
                var category = categories[i % categories.Length];
                
                talks.Add(new Talk
                {
                    Day = day,
                    StartTime = startTime,
                    EndTime = endTime,
                    Room = room,
                    Title = title,
                    Speaker = speaker,
                    Category = category
                });
            }
            
            return talks;
        }
    }

    // Helper class for testing
    public class TalkDisplayItem
    {
        public Talk Talk { get; set; } = new Talk();
        public bool IsSelected { get; set; }
    }
}