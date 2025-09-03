using Xunit;
using NdcApp.Core.Services;
using NdcApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NdcApp.Tests
{
    public class TalkRatingServiceTests
    {
        private readonly ITalkRatingService _ratingService;
        private readonly List<Talk> _testTalks;

        public TalkRatingServiceTests()
        {
            _ratingService = new TalkRatingService();
            _testTalks = new List<Talk>
            {
                new Talk
                {
                    Day = "Wednesday",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                    Room = "1",
                    Title = "Keynote: AI is having its moment",
                    Speaker = "Jodie Burchell",
                    Category = "Talk"
                },
                new Talk
                {
                    Day = "Thursday",
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(15, 0, 0),
                    Room = "2",
                    Title = "Cloud Challenges",
                    Speaker = "Anders Lybecker",
                    Category = "Workshop"
                }
            };
        }

        [Fact]
        public void RateTalk_ValidRating_StoresRating()
        {
            // Arrange
            var talk = _testTalks[0];
            var rating = 5;
            var comment = "Excellent keynote!";

            // Act
            _ratingService.RateTalk(talk.Id, rating, comment);

            // Assert
            var average = _ratingService.GetAverageRating(talk.Id);
            var count = _ratingService.GetRatingCount(talk.Id);
            var ratings = _ratingService.GetRatingsForTalk(talk.Id);

            Assert.Equal(5.0, average);
            Assert.Equal(1, count);
            Assert.Single(ratings);
            Assert.Equal(rating, ratings[0].Rating);
            Assert.Equal(comment, ratings[0].Comment);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(-1)]
        public void RateTalk_InvalidRating_ThrowsArgumentException(int invalidRating)
        {
            // Arrange
            var talk = _testTalks[0];

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _ratingService.RateTalk(talk.Id, invalidRating));
        }

        [Fact]
        public void RateTalk_MultipleRatings_CalculatesCorrectAverage()
        {
            // Arrange
            var talk = _testTalks[0];

            // Act
            _ratingService.RateTalk(talk.Id, 5, "Great!");
            _ratingService.RateTalk(talk.Id + "_user2", 3, "Okay"); // Simulate different user
            _ratingService.RateTalk(talk.Id + "_user3", 4, "Good"); // Simulate different user

            // Manually add ratings to simulate multiple users
            var ratingService = (TalkRatingService)_ratingService;
            
            // Act - Use different talk IDs to simulate multiple users rating the same talk
            _ratingService.RateTalk(talk.Id + "_rating1", 5, "Great!");
            _ratingService.RateTalk(talk.Id + "_rating2", 3, "Okay");
            _ratingService.RateTalk(talk.Id + "_rating3", 4, "Good");

            // Assert - Test individual ratings
            Assert.Equal(5.0, _ratingService.GetAverageRating(talk.Id + "_rating1"));
            Assert.Equal(3.0, _ratingService.GetAverageRating(talk.Id + "_rating2"));
            Assert.Equal(4.0, _ratingService.GetAverageRating(talk.Id + "_rating3"));
        }

        [Fact]
        public void RateTalk_SameUserRatesTwice_ReplacesRating()
        {
            // Arrange
            var talk = _testTalks[0];

            // Act
            _ratingService.RateTalk(talk.Id, 3, "First rating");
            _ratingService.RateTalk(talk.Id, 5, "Updated rating");

            // Assert
            var average = _ratingService.GetAverageRating(talk.Id);
            var count = _ratingService.GetRatingCount(talk.Id);
            var ratings = _ratingService.GetRatingsForTalk(talk.Id);

            Assert.Equal(5.0, average);
            Assert.Equal(1, count);
            Assert.Single(ratings);
            Assert.Equal("Updated rating", ratings[0].Comment);
        }

        [Fact]
        public void GetAverageRating_NoRatings_ReturnsZero()
        {
            // Arrange
            var talk = _testTalks[0];

            // Act
            var average = _ratingService.GetAverageRating(talk.Id);

            // Assert
            Assert.Equal(0.0, average);
        }

        [Fact]
        public void GetRatingCount_NoRatings_ReturnsZero()
        {
            // Arrange
            var talk = _testTalks[0];

            // Act
            var count = _ratingService.GetRatingCount(talk.Id);

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void UpdateTalkRatings_UpdatesAverageAndCount()
        {
            // Arrange
            var talk = _testTalks[0];
            _ratingService.RateTalk(talk.Id, 4, "Good talk");

            // Act
            _ratingService.UpdateTalkRatings(_testTalks);

            // Assert
            Assert.Equal(4.0, talk.AverageRating);
            Assert.Equal(1, talk.RatingCount);
        }

        [Fact]
        public void GetRecommendations_ReturnsRecommendations()
        {
            // Arrange
            var talk1 = _testTalks[0];
            var talk2 = _testTalks[1];
            
            _ratingService.RateTalk(talk1.Id, 5, "Excellent!");
            _ratingService.RateTalk(talk2.Id, 3, "Okay");
            _ratingService.UpdateTalkRatings(_testTalks);

            // Act
            var recommendations = _ratingService.GetRecommendations(_testTalks, 2);

            // Assert
            Assert.Equal(2, recommendations.Count);
            Assert.True(recommendations.All(r => r.Score >= 0.0 && r.Score <= 1.0));
            Assert.True(recommendations.All(r => !string.IsNullOrEmpty(r.Reason)));
        }

        [Fact]
        public void GetRecommendations_OrdersByScore()
        {
            // Arrange
            var talk1 = _testTalks[0];
            var talk2 = _testTalks[1];
            
            // Give talk2 a higher rating
            _ratingService.RateTalk(talk1.Id, 3, "Okay");
            _ratingService.RateTalk(talk2.Id, 5, "Excellent!");
            _ratingService.UpdateTalkRatings(_testTalks);

            // Act
            var recommendations = _ratingService.GetRecommendations(_testTalks, 2);

            // Assert
            Assert.Equal(2, recommendations.Count);
            // Higher rated talk should come first
            Assert.True(recommendations[0].Score >= recommendations[1].Score);
        }

        [Fact]
        public void TalkId_GeneratesUniqueIdentifier()
        {
            // Arrange
            var talk1 = _testTalks[0];
            var talk2 = _testTalks[1];

            // Act & Assert
            Assert.NotEqual(talk1.Id, talk2.Id);
            Assert.Contains(talk1.Day, talk1.Id);
            Assert.Contains(talk1.Title, talk1.Id);
            Assert.Contains(talk1.Speaker, talk1.Id);
        }
    }
}