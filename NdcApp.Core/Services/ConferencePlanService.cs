using System;
using System.Collections.Generic;
using System.Linq;
using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    public class ConferencePlanService
    {
        private Dictionary<string, Talk> selectedTalks = new();

        public void SelectTalk(Talk talk)
        {
            if (talk == null) return;
            var key = $"{talk.Day}|{talk.StartTime}";
            selectedTalks[key] = talk;
        }

        public void DeselectTalk(Talk talk)
        {
            if (talk == null) return;
            var key = $"{talk.Day}|{talk.StartTime}";
            selectedTalks.Remove(key);
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
                $"{t.Day},{t.StartTime},{t.EndTime},{t.Room},{t.Title},{t.Speaker},{t.Category}"));
        }

        public void DeserializeSelectedTalks(string serializedTalks)
        {
            selectedTalks.Clear();
            if (string.IsNullOrEmpty(serializedTalks)) return;

            var selected = serializedTalks.Split('|')
                .Select(line => line.Split(','))
                .Where(parts => parts.Length == 7)
                .Select(parts => new Talk {
                    Day = parts[0],
                    StartTime = TimeSpan.Parse(parts[1]),
                    EndTime = TimeSpan.Parse(parts[2]),
                    Room = parts[3],
                    Title = parts[4],
                    Speaker = parts[5],
                    Category = parts[6]
                });

            foreach (var talk in selected)
            {
                var key = $"{talk.Day}|{talk.StartTime}";
                selectedTalks[key] = talk;
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
    }
}