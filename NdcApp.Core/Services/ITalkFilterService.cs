using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    /// <summary>
    /// Service interface for filtering and searching through conference talks.
    /// </summary>
    public interface ITalkFilterService
    {
        /// <summary>
        /// Filters a list of talks based on the provided search text.
        /// </summary>
        /// <param name="talks">The list of talks to filter.</param>
        /// <param name="searchText">The search text to filter by. Searches in title, speaker, category, and room fields.</param>
        /// <returns>A filtered list of talks that match the search criteria.</returns>
        List<Talk> FilterTalks(List<Talk> talks, string searchText);
    }
}