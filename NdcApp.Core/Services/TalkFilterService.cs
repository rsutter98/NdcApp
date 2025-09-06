using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    /// <summary>
    /// Implementation of talk filter service for searching and filtering conference talks.
    /// </summary>
    public class TalkFilterService : ITalkFilterService
    {
        /// <summary>
        /// Filters talks based on search text matching title, speaker, category, or room.
        /// </summary>
        /// <param name="talks">The list of talks to filter.</param>
        /// <param name="searchText">The search text to filter by. Case-insensitive search.</param>
        /// <returns>A filtered list of talks matching the search criteria.</returns>
        public List<Talk> FilterTalks(List<Talk> talks, string searchText)
        {
            if (talks == null)
            {
                return new List<Talk>();
            }

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return talks;
            }

            var lowerSearchText = searchText.ToLower(CultureInfo.CurrentCulture);
            return talks.Where(talk => ContainsSearchText(talk, lowerSearchText)).ToList();
        }

        private static bool ContainsSearchText(Talk talk, string lowerSearchText)
        {
            return talk.Title.ToLower(CultureInfo.CurrentCulture).Contains(lowerSearchText, StringComparison.Ordinal) ||
                   talk.Speaker.ToLower(CultureInfo.CurrentCulture).Contains(lowerSearchText, StringComparison.Ordinal) ||
                   talk.Category.ToLower(CultureInfo.CurrentCulture).Contains(lowerSearchText, StringComparison.Ordinal) ||
                   talk.Room.ToLower(CultureInfo.CurrentCulture).Contains(lowerSearchText, StringComparison.Ordinal);
        }
    }
}