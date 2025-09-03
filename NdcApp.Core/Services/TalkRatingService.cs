using System;
using System.Collections.Generic;
using System.Linq;
using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    public interface ITalkRatingService
    {
        void RateTalk(string talkId, int rating, string comment = "");
        double GetAverageRating(string talkId);
        int GetRatingCount(string talkId);
        List<TalkRating> GetRatingsForTalk(string talkId);
        List<TalkRecommendation> GetRecommendations(List<Talk> talks, int maxRecommendations = 5);
        void UpdateTalkRatings(List<Talk> talks);
    }

    public class TalkRatingService : ITalkRatingService
    {
        private readonly List<TalkRating> _ratings = new();

        public void RateTalk(string talkId, int rating, string comment = "")
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");

            // Remove existing rating for this talk by this user
            _ratings.RemoveAll(r => r.TalkId == talkId && r.UserId == "default");

            // Add new rating
            _ratings.Add(new TalkRating
            {
                TalkId = talkId,
                Rating = rating,
                Comment = comment,
                RatingDate = DateTime.Now,
                UserId = "default"
            });
        }

        public double GetAverageRating(string talkId)
        {
            var talkRatings = _ratings.Where(r => r.TalkId == talkId).ToList();
            return talkRatings.Any() ? talkRatings.Average(r => r.Rating) : 0.0;
        }

        public int GetRatingCount(string talkId)
        {
            return _ratings.Count(r => r.TalkId == talkId);
        }

        public List<TalkRating> GetRatingsForTalk(string talkId)
        {
            return _ratings.Where(r => r.TalkId == talkId).OrderByDescending(r => r.RatingDate).ToList();
        }

        public void UpdateTalkRatings(List<Talk> talks)
        {
            foreach (var talk in talks)
            {
                talk.AverageRating = GetAverageRating(talk.Id);
                talk.RatingCount = GetRatingCount(talk.Id);
            }
        }

        public List<TalkRecommendation> GetRecommendations(List<Talk> talks, int maxRecommendations = 5)
        {
            var recommendations = new List<TalkRecommendation>();

            foreach (var talk in talks)
            {
                var score = CalculateRecommendationScore(talk);
                var reason = GenerateRecommendationReason(talk);

                recommendations.Add(new TalkRecommendation
                {
                    Talk = talk,
                    Score = score,
                    Reason = reason
                });
            }

            return recommendations
                .OrderByDescending(r => r.Score)
                .Take(maxRecommendations)
                .ToList();
        }

        private double CalculateRecommendationScore(Talk talk)
        {
            double score = 0.0;

            // Base score from average rating (40% weight)
            if (talk.RatingCount > 0)
            {
                score += (talk.AverageRating / 5.0) * 0.4;
            }

            // Popularity score from number of ratings (20% weight)
            var maxRatings = _ratings.GroupBy(r => r.TalkId).Max(g => g.Count());
            if (maxRatings > 0)
            {
                score += (talk.RatingCount / (double)maxRatings) * 0.2;
            }

            // Category diversity bonus (20% weight)
            var popularCategories = GetPopularCategories();
            if (popularCategories.Contains(talk.Category))
            {
                score += 0.2;
            }

            // Recent activity bonus (20% weight)
            var recentRatings = _ratings.Where(r => r.TalkId == talk.Id && r.RatingDate > DateTime.Now.AddDays(-30));
            if (recentRatings.Any())
            {
                score += 0.2;
            }

            return Math.Min(score, 1.0); // Cap at 1.0
        }

        private string GenerateRecommendationReason(Talk talk)
        {
            var reasons = new List<string>();

            if (talk.AverageRating >= 4.0 && talk.RatingCount > 0)
            {
                reasons.Add($"Highly rated ({talk.AverageRating:F1}★)");
            }

            if (talk.RatingCount > 5)
            {
                reasons.Add("Popular choice");
            }

            var popularCategories = GetPopularCategories();
            if (popularCategories.Contains(talk.Category))
            {
                reasons.Add($"Trending {talk.Category}");
            }

            if (!reasons.Any())
            {
                reasons.Add("Might interest you");
            }

            return string.Join(", ", reasons);
        }

        private List<string> GetPopularCategories()
        {
            // For now, return some default popular categories
            // In a real implementation, this would analyze actual talk data
            return new List<string> { "Talk", "Workshop", "Keynote" };
        }
    }
}