using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    /// <summary>
    /// Service for managing conference plan operations including talk selection, 
    /// serialization, and organization functionality.
    /// </summary>
    public interface IConferencePlanService
    {
        /// <summary>
        /// Selects a talk for the conference plan. If another talk is already selected 
        /// for the same time slot, it will be replaced.
        /// </summary>
        /// <param name="talk">The talk to select. Null values are ignored.</param>
        void SelectTalk(Talk? talk);
        
        /// <summary>
        /// Removes a talk from the selected conference plan.
        /// </summary>
        /// <param name="talk">The talk to deselect. Null values are ignored.</param>
        void DeselectTalk(Talk? talk);
        
        /// <summary>
        /// Checks if a specific talk is currently selected in the conference plan.
        /// </summary>
        /// <param name="talk">The talk to check. Null values return false.</param>
        /// <returns>True if the talk is selected, false otherwise.</returns>
        bool IsTalkSelected(Talk? talk);
        
        /// <summary>
        /// Gets all currently selected talks.
        /// </summary>
        /// <returns>A list of all selected Talk objects.</returns>
        List<Talk> GetSelectedTalks();
        
        /// <summary>
        /// Clears all selected talks from the conference plan.
        /// </summary>
        void ClearSelectedTalks();
        
        /// <summary>
        /// Serializes the currently selected talks to a string for persistence.
        /// </summary>
        /// <returns>A serialized string representation of selected talks.</returns>
        string SerializeSelectedTalks();
        
        /// <summary>
        /// Deserializes and restores selected talks from a serialized string.
        /// </summary>
        /// <param name="serializedTalks">The serialized talks string. Null or empty values clear the selection.</param>
        void DeserializeSelectedTalks(string? serializedTalks);
        
        /// <summary>
        /// Gets the next scheduled talk from the selected talks based on current time.
        /// </summary>
        /// <returns>The next Talk object, or null if no future talks are selected.</returns>
        Talk? GetNextSelectedTalk();
        
        /// <summary>
        /// Sorts a list of talks by speaker name in alphabetical order.
        /// </summary>
        /// <param name="talks">The talks to sort.</param>
        /// <returns>A sorted list of talks by speaker.</returns>
        List<Talk> SortTalksBySpeaker(List<Talk> talks);
        
        /// <summary>
        /// Sorts a list of talks by category in alphabetical order.
        /// </summary>
        /// <param name="talks">The talks to sort.</param>
        /// <returns>A sorted list of talks by category.</returns>
        List<Talk> SortTalksByCategory(List<Talk> talks);
        
        /// <summary>
        /// Sorts a list of talks by their average rating in descending order.
        /// </summary>
        /// <param name="talks">The talks to sort.</param>
        /// <returns>A sorted list of talks by rating.</returns>
        List<Talk> SortTalksByRating(List<Talk> talks);
        
        /// <summary>
        /// Sorts a list of talks by day and then by start time (standard chronological order).
        /// </summary>
        /// <param name="talks">The talks to sort.</param>
        /// <returns>A sorted list of talks in chronological order.</returns>
        List<Talk> SortTalksStandard(List<Talk> talks);
        
        /// <summary>
        /// Gets talk recommendations based on ratings and user preferences.
        /// </summary>
        /// <param name="talks">The pool of talks to generate recommendations from.</param>
        /// <param name="maxRecommendations">Maximum number of recommendations to return.</param>
        /// <returns>A list of recommended talks.</returns>
        List<TalkRecommendation> GetRecommendations(List<Talk> talks, int maxRecommendations = 5);
        
        /// <summary>
        /// Adds a rating for a specific talk.
        /// </summary>
        /// <param name="talk">The talk to rate. Null values are ignored.</param>
        /// <param name="rating">The rating value (typically 1-5).</param>
        /// <param name="comment">Optional comment for the rating.</param>
        void RateTalk(Talk? talk, int rating, string comment = "");
        
        /// <summary>
        /// Gets all ratings for a specific talk.
        /// </summary>
        /// <param name="talk">The talk to get ratings for. Null values return empty list.</param>
        /// <returns>A list of ratings for the specified talk.</returns>
        List<TalkRating> GetRatingsForTalk(Talk? talk);
        
        /// <summary>
        /// Updates the rating properties for all talks in the provided list.
        /// </summary>
        /// <param name="talks">The talks to update rating information for.</param>
        void UpdateAllTalkRatings(List<Talk> talks);
    }
}