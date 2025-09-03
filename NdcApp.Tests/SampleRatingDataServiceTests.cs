using Xunit;
using NdcApp.Core.Services;
using NdcApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NdcApp.Tests
{
    public class SampleRatingDataServiceTests
    {
        [Fact]
        public void LoadSampleRatingData_WithValidTalks_AddsSampleRatings()
        {
            // Arrange
            var ratingService = new TalkRatingService();
            var talks = new List<Talk>
            {
                new Talk
                {
                    Day = "Wednesday",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                    Room = "1",
                    Title = "Test Talk 1",
                    Speaker = "Speaker 1",
                    Category = "Talk"
                },
                new Talk
                {
                    Day = "Wednesday",
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(11, 0, 0),
                    Room = "2",
                    Title = "Test Talk 2",
                    Speaker = "Speaker 2",
                    Category = "Workshop"
                }
            };

            // Act
            SampleRatingDataService.LoadSampleRatingData(ratingService, talks);

            // Assert
            // Check that talks have been assigned ratings
            Assert.True(talks.Any(t => t.AverageRating > 0));
            Assert.True(talks.Any(t => t.RatingCount > 0));
            
            // Verify ratings are in expected range
            foreach (var talk in talks.Where(t => t.RatingCount > 0))
            {
                Assert.True(talk.AverageRating >= 3.0 && talk.AverageRating <= 5.0);
                Assert.True(talk.RatingCount >= 1 && talk.RatingCount <= 5);
            }
        }

        [Fact]
        public void LoadSampleRatingData_WithEmptyTalksList_DoesNotCrash()
        {
            // Arrange
            var ratingService = new TalkRatingService();
            var talks = new List<Talk>();

            // Act & Assert (should not throw)
            SampleRatingDataService.LoadSampleRatingData(ratingService, talks);
        }

        [Fact]
        public void LoadSampleRatingData_WithManyTalks_OnlyRatesFirst10()
        {
            // Arrange
            var ratingService = new TalkRatingService();
            var talks = new List<Talk>();
            
            // Create 15 talks
            for (int i = 0; i < 15; i++)
            {
                talks.Add(new Talk
                {
                    Day = "Wednesday",
                    StartTime = new TimeSpan(9 + i, 0, 0),
                    EndTime = new TimeSpan(10 + i, 0, 0),
                    Room = i.ToString(),
                    Title = $"Test Talk {i}",
                    Speaker = $"Speaker {i}",
                    Category = "Talk"
                });
            }

            // Act
            SampleRatingDataService.LoadSampleRatingData(ratingService, talks);

            // Assert
            var ratedTalks = talks.Where(t => t.RatingCount > 0).ToList();
            Assert.True(ratedTalks.Count <= 10, "Should not rate more than 10 talks");
        }
    }
}