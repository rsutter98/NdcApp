using Xunit;
using NdcApp.Core.Services;
using NdcApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NdcApp.Tests
{
    public class ConferencePlanServiceRatingTests
    {
        private ConferencePlanService CreateService()
        {
            return new ConferencePlanService();
        }

        private Talk CreateTestTalk(string day = "Wednesday", string startTime = "09:00", string endTime = "10:00", 
            string room = "1", string title = "Test Talk", string speaker = "Test Speaker", string category = "Talk")
        {
            return new Talk
            {
                Day = day,
                StartTime = TimeSpan.Parse(startTime),
                EndTime = TimeSpan.Parse(endTime),
                Room = room,
                Title = title,
                Speaker = speaker,
                Category = category
            };
        }

        [Fact]
        public void RateTalk_ValidRating_StoresRating()
        {
            // Arrange
            var service = CreateService();
            var talk = CreateTestTalk();

            // Act
            service.RateTalk(talk, 5, "Excellent talk!");

            // Assert
            var ratings = service.GetRatingsForTalk(talk);
            Assert.Single(ratings);
            Assert.Equal(5, ratings[0].Rating);
            Assert.Equal("Excellent talk!", ratings[0].Comment);
        }

        [Fact]
        public void GetRecommendations_WithRatedTalks_ReturnsRecommendations()
        {
            // Arrange
            var service = CreateService();
            var talks = new List<Talk>
            {
                CreateTestTalk(title: "Talk 1"),
                CreateTestTalk(title: "Talk 2", startTime: "10:00", endTime: "11:00"),
                CreateTestTalk(title: "Talk 3", startTime: "11:00", endTime: "12:00")
            };

            // Rate some talks
            service.RateTalk(talks[0], 5, "Great!");
            service.RateTalk(talks[1], 3, "Okay");

            // Act
            var recommendations = service.GetRecommendations(talks, 3);

            // Assert
            Assert.Equal(3, recommendations.Count);
            Assert.All(recommendations, r => Assert.NotNull(r.Talk));
            Assert.All(recommendations, r => Assert.True(r.Score >= 0.0 && r.Score <= 1.0));
            Assert.All(recommendations, r => Assert.False(string.IsNullOrEmpty(r.Reason)));
        }

        [Fact]
        public void SortTalksByRating_OrdersByRatingDescending()
        {
            // Arrange
            var service = CreateService();
            var talks = new List<Talk>
            {
                CreateTestTalk(title: "Low Rated Talk"),
                CreateTestTalk(title: "High Rated Talk", startTime: "10:00", endTime: "11:00"),
                CreateTestTalk(title: "Medium Rated Talk", startTime: "11:00", endTime: "12:00")
            };

            // Set ratings manually for testing
            talks[0].AverageRating = 2.0;
            talks[0].RatingCount = 1;
            talks[1].AverageRating = 5.0;
            talks[1].RatingCount = 3;
            talks[2].AverageRating = 3.5;
            talks[2].RatingCount = 2;

            // Act
            var sortedTalks = service.SortTalksByRating(talks);

            // Assert
            Assert.Equal("High Rated Talk", sortedTalks[0].Title);
            Assert.Equal("Medium Rated Talk", sortedTalks[1].Title);
            Assert.Equal("Low Rated Talk", sortedTalks[2].Title);
        }

        [Fact]
        public void UpdateAllTalkRatings_UpdatesRatingProperties()
        {
            // Arrange
            var service = CreateService();
            var talks = new List<Talk>
            {
                CreateTestTalk(title: "Talk to Rate")
            };

            service.RateTalk(talks[0], 4, "Good talk");

            // Act
            service.UpdateAllTalkRatings(talks);

            // Assert
            Assert.Equal(4.0, talks[0].AverageRating);
            Assert.Equal(1, talks[0].RatingCount);
        }

        [Fact]
        public void SerializeSelectedTalks_IncludesRatingData()
        {
            // Arrange
            var service = CreateService();
            var talk = CreateTestTalk();
            talk.AverageRating = 4.5;
            talk.RatingCount = 3;

            service.SelectTalk(talk);

            // Act
            var serialized = service.SerializeSelectedTalks();

            // Assert
            Assert.Contains("4.5", serialized);
            Assert.Contains("3", serialized);
        }

        [Fact]
        public void DeserializeSelectedTalks_WithRatingData_RestoresRatings()
        {
            // Arrange
            var service = CreateService();
            var serialized = "Wednesday,09:00:00,10:00:00,1,Test Talk,Test Speaker,Talk,4.5,3";

            // Act
            service.DeserializeSelectedTalks(serialized);

            // Assert
            var selectedTalks = service.GetSelectedTalks();
            Assert.Single(selectedTalks);
            var talk = selectedTalks[0];
            Assert.Equal(4.5, talk.AverageRating);
            Assert.Equal(3, talk.RatingCount);
        }

        [Fact]
        public void DeserializeSelectedTalks_OldFormat_StillWorks()
        {
            // Arrange
            var service = CreateService();
            var serialized = "Wednesday,09:00:00,10:00:00,1,Test Talk,Test Speaker,Talk"; // Old format without ratings

            // Act
            service.DeserializeSelectedTalks(serialized);

            // Assert
            var selectedTalks = service.GetSelectedTalks();
            Assert.Single(selectedTalks);
            var talk = selectedTalks[0];
            Assert.Equal(0.0, talk.AverageRating);
            Assert.Equal(0, talk.RatingCount);
            Assert.Equal("Test Talk", talk.Title);
        }
    }
}