using System;
using System.Collections.Generic;
using System.Linq;
using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    public class TalkFilterService : ITalkFilterService
    {
        public List<Talk> FilterTalks(List<Talk> talks, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return talks;

            var lowerSearchText = searchText.ToLower();
            return talks.Where(t => 
                t.Title.ToLower().Contains(lowerSearchText) ||
                t.Speaker.ToLower().Contains(lowerSearchText) ||
                t.Category.ToLower().Contains(lowerSearchText) ||
                t.Room.ToLower().Contains(lowerSearchText)
            ).ToList();
        }
    }
}