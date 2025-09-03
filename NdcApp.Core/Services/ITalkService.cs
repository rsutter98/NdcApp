using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    /// <summary>
    /// Service for loading and parsing talk data from various sources.
    /// </summary>
    public interface ITalkService
    {
        /// <summary>
        /// Loads talks from a CSV file at the specified path.
        /// </summary>
        /// <param name="csvPath">The path to the CSV file containing talk data.</param>
        /// <returns>A list of parsed Talk objects.</returns>
        List<Talk> LoadTalks(string csvPath);
        
        /// <summary>
        /// Parses talks from CSV content string.
        /// </summary>
        /// <param name="csvContent">The CSV content as a string.</param>
        /// <returns>A list of parsed Talk objects.</returns>
        List<Talk> ParseTalksFromString(string csvContent);
    }
}