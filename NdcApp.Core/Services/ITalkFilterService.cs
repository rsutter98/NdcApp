using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    public interface ITalkFilterService
    {
        List<Talk> FilterTalks(List<Talk> talks, string searchText);
    }
}