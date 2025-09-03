using Xunit;
using NdcApp.Core.Services;
using NdcApp.Core.Models;
using System;
using System.Collections.Generic;

namespace NdcApp.Tests
{
    public class TalkFilterServiceTests
    {
        private readonly List<Talk> testTalks = new List<Talk>
        {
            new Talk
            {
                Day = "Wednesday",
                StartTime = TimeSpan.Parse("09:00"),
                EndTime = TimeSpan.Parse("10:00"),
                Room = "Room 1",
                Title = "Keynote: AI is having its moment",
                Speaker = "Jodie Burchell",
                Category = "Keynote"
            },
            new Talk
            {
                Day = "Wednesday",
                StartTime = TimeSpan.Parse("10:20"),
                EndTime = TimeSpan.Parse("11:20"),
                Room = "Room 2",
                Title = "Java Sucks (So C# Didn't Have To)",
                Speaker = "Adele Carpenter",
                Category = "Programming"
            },
            new Talk
            {
                Day = "Thursday",
                StartTime = TimeSpan.Parse("14:00"),
                EndTime = TimeSpan.Parse("15:00"),
                Room = "Room 3",
                Title = "The future of cloud computing",
                Speaker = "Anders Lybecker",
                Category = "Cloud"
            }
        };

        [Fact]
        public void FilterTalks_EmptySearchText_ReturnsAllTalks()
        {
            // Act
            var result = TalkFilterService.FilterTalks(testTalks, "");

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(testTalks, result);
        }

        [Fact]
        public void FilterTalks_NullSearchText_ReturnsAllTalks()
        {
            // Act
            var result = TalkFilterService.FilterTalks(testTalks, null!);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(testTalks, result);
        }

        [Fact]
        public void FilterTalks_SearchByTitle_ReturnsMatchingTalks()
        {
            // Act
            var result = TalkFilterService.FilterTalks(testTalks, "AI");

            // Assert
            Assert.Single(result);
            Assert.Equal("Keynote: AI is having its moment", result[0].Title);
        }

        [Fact]
        public void FilterTalks_SearchBySpeaker_ReturnsMatchingTalks()
        {
            // Act
            var result = TalkFilterService.FilterTalks(testTalks, "Jodie");

            // Assert
            Assert.Single(result);
            Assert.Equal("Jodie Burchell", result[0].Speaker);
        }

        [Fact]
        public void FilterTalks_SearchByCategory_ReturnsMatchingTalks()
        {
            // Act
            var result = TalkFilterService.FilterTalks(testTalks, "Cloud");

            // Assert
            Assert.Single(result);
            Assert.Equal("Cloud", result[0].Category);
        }

        [Fact]
        public void FilterTalks_SearchByRoom_ReturnsMatchingTalks()
        {
            // Act
            var result = TalkFilterService.FilterTalks(testTalks, "Room 2");

            // Assert
            Assert.Single(result);
            Assert.Equal("Room 2", result[0].Room);
        }

        [Fact]
        public void FilterTalks_CaseInsensitiveSearch_ReturnsMatchingTalks()
        {
            // Act
            var result = TalkFilterService.FilterTalks(testTalks, "java");

            // Assert
            Assert.Single(result);
            Assert.Equal("Java Sucks (So C# Didn't Have To)", result[0].Title);
        }

        [Fact]
        public void FilterTalks_NoMatch_ReturnsEmptyList()
        {
            // Act
            var result = TalkFilterService.FilterTalks(testTalks, "NonExistentTerm");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterTalks_PartialMatch_ReturnsMatchingTalks()
        {
            // Act
            var result = TalkFilterService.FilterTalks(testTalks, "future");

            // Assert
            Assert.Single(result);
            Assert.Equal("The future of cloud computing", result[0].Title);
        }
    }
}