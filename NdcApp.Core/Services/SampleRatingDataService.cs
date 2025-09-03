using NdcApp.Core.Models;
using NdcApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NdcApp.Core.Services
{
    public static class SampleRatingDataService
    {
        public static void LoadSampleRatingData(ITalkRatingService ratingService, List<Talk> talks)
        {
            if (talks.Count == 0) return;

            // Add some sample ratings to demonstrate the system
            var random = new Random(42); // Fixed seed for consistent demo data

            foreach (var talk in talks.Take(Math.Min(10, talks.Count))) // Rate first 10 talks
            {
                // Simulate 1-5 ratings per talk by creating separate ratings
                var ratingsCount = random.Next(1, 6);
                
                for (int i = 0; i < ratingsCount; i++)
                {
                    var rating = random.Next(2, 6); // Ratings between 2-5 for demo
                    var comments = GetSampleComment(rating);
                    
                    // Create unique rating by using a simulated user ID
                    try
                    {
                        // Use the rating service to add individual ratings
                        // For demo purposes, we'll create artificial talk IDs
                        var talkWithUserSuffix = $"{talk.Id}_demo{i}";
                        ratingService.RateTalk(talkWithUserSuffix, rating, comments);
                    }
                    catch
                    {
                        // Continue if rating fails
                        continue;
                    }
                }

                // Calculate and set the average for the actual talk
                // We need to manually calculate since our demo creates separate IDs
                var avgRating = random.NextDouble() * 2 + 3; // 3.0 to 5.0 range
                var ratingCount = ratingsCount;
                
                talk.AverageRating = Math.Round(avgRating, 1);
                talk.RatingCount = ratingCount;
            }
        }

        private static string GetSampleComment(int rating)
        {
            return rating switch
            {
                5 => "Excellent presentation! Learned a lot.",
                4 => "Very good content, well presented.",
                3 => "Good talk, some useful information.",
                2 => "Okay, but could be better organized.",
                1 => "Not very helpful for my needs.",
                _ => "Good talk"
            };
        }
    }
}