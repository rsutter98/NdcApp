using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    public interface ITalkService
    {
        List<Talk> LoadTalks(string csvPath);
        List<Talk> ParseTalksFromString(string csvContent);
    }
}