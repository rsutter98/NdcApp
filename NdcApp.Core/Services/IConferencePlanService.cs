using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    public interface IConferencePlanService
    {
        void SelectTalk(Talk talk);
        void DeselectTalk(Talk talk);
        bool IsTalkSelected(Talk talk);
        List<Talk> GetSelectedTalks();
        void ClearSelectedTalks();
        string SerializeSelectedTalks();
        void DeserializeSelectedTalks(string serializedTalks);
        Talk? GetNextSelectedTalk();
        List<Talk> SortTalksBySpeaker(List<Talk> talks);
        List<Talk> SortTalksByCategory(List<Talk> talks);
        List<Talk> SortTalksByRating(List<Talk> talks);
        List<Talk> SortTalksStandard(List<Talk> talks);
        List<TalkRecommendation> GetRecommendations(List<Talk> talks, int maxRecommendations = 5);
        void RateTalk(Talk talk, int rating, string comment = "");
        List<TalkRating> GetRatingsForTalk(Talk talk);
        void UpdateAllTalkRatings(List<Talk> talks);
    }
}