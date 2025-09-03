using System;
using System.Collections.Generic;
using System.Linq;
using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    public class ConferencePlanService : IConferencePlanService
    {
        private Dictionary<string, Talk> selectedTalks = new();
        private readonly ITalkRatingService _ratingService;
        private readonly ILoggerService _logger;

        public ConferencePlanService(ITalkRatingService ratingService, ILoggerService logger)
        {
            _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SelectTalk(Talk talk)
        {
            if (talk == null) return;
            var key = $"{talk.Day}|{talk.StartTime}";
            selectedTalks[key] = talk;
            _logger.LogInfo($"Talk selected: {talk.Title} by {talk.Speaker}");
        }

        public void DeselectTalk(Talk talk)
        {
            if (talk == null) return;
            var key = $"{talk.Day}|{talk.StartTime}";
            selectedTalks.Remove(key);
            _logger.LogInfo($"Talk deselected: {talk.Title} by {talk.Speaker}");
        }

        public bool IsTalkSelected(Talk talk)
        {
            if (talk == null) return false;
            var key = $"{talk.Day}|{talk.StartTime}";
            return selectedTalks.ContainsKey(key) && selectedTalks[key].Title == talk.Title;
        }

        public List<Talk> GetSelectedTalks()
        {
            return selectedTalks.Values.ToList();
        }

        public void ClearSelectedTalks()
        {
            selectedTalks.Clear();
        }

        public string SerializeSelectedTalks()
        {
            return string.Join("|", selectedTalks.Values.Select(t => 
                $"{t.Day},{t.StartTime},{t.EndTime},{t.Room},{t.Title},{t.Speaker},{t.Category},{t.AverageRating},{t.RatingCount}"));
        }

        public void DeserializeSelectedTalks(string serializedTalks)
        {
            selectedTalks.Clear();
            if (string.IsNullOrEmpty(serializedTalks)) return;

            try
            {
                var selected = serializedTalks.Split('|')
                    .Select(line => line.Split(','))
                    .Where(parts => parts.Length >= 7) // Support both old and new format
                    .Select(parts => new Talk {
                        Day = parts[0],
                        StartTime = TimeSpan.Parse(parts[1]),
                        EndTime = TimeSpan.Parse(parts[2]),
                        Room = parts[3],
                        Title = parts[4],
                        Speaker = parts[5],
                        Category = parts[6],
                        AverageRating = parts.Length > 7 && double.TryParse(parts[7], out var rating) ? rating : 0.0,
                        RatingCount = parts.Length > 8 && int.TryParse(parts[8], out var count) ? count : 0
                    });

                foreach (var talk in selected)
                {
                    var key = $"{talk.Day}|{talk.StartTime}";
                    selectedTalks[key] = talk;
                }
                
                _logger.LogInfo($"Deserialized {selectedTalks.Count} selected talks");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize selected talks");
                throw;
            }
        }

        public Talk? GetNextSelectedTalk()
        {
            var now = DateTime.Now.TimeOfDay;
            return selectedTalks.Values
                .OrderBy(t => t.Day)
                .ThenBy(t => t.StartTime)
                .FirstOrDefault(t => t.StartTime > now);
        }

        public List<Talk> SortTalksBySpeaker(List<Talk> talks)
        {
            return talks.OrderBy(t => t.Speaker).ToList();
        }

        public List<Talk> SortTalksByCategory(List<Talk> talks)
        {
            return talks.OrderBy(t => t.Category).ToList();
        }

        public List<Talk> SortTalksByRating(List<Talk> talks)
        {
            return talks.OrderByDescending(t => t.AverageRating)
                       .ThenByDescending(t => t.RatingCount)
                       .ToList();
        }

        public List<Talk> SortTalksStandard(List<Talk> talks)
        {
            // Define day order for proper chronological sorting
            var dayOrder = new Dictionary<string, int>
            {
                { "Monday", 1 },
                { "Tuesday", 2 },
                { "Wednesday", 3 },
                { "Thursday", 4 },
                { "Friday", 5 },
                { "Saturday", 6 },
                { "Sunday", 7 }
            };

            return talks.OrderBy(t => dayOrder.GetValueOrDefault(t.Day, 999))
                       .ThenBy(t => t.StartTime)
                       .ToList();
        }

        public List<TalkRecommendation> GetRecommendations(List<Talk> talks, int maxRecommendations = 5)
        {
            _ratingService.UpdateTalkRatings(talks);
            return _ratingService.GetRecommendations(talks, maxRecommendations);
        }

        public void RateTalk(Talk talk, int rating, string comment = "")
        {
            _ratingService.RateTalk(talk.Id, rating, comment);
        }

        public List<TalkRating> GetRatingsForTalk(Talk talk)
        {
            return _ratingService.GetRatingsForTalk(talk.Id);
        }

        public void UpdateAllTalkRatings(List<Talk> talks)
        {
            _ratingService.UpdateTalkRatings(talks);
        }
    }
}