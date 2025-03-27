using static ValorantStatusWebView.Components.Shared.RegionStatusCard;
using ValorantStatusWebView.Models;

namespace ValorantStatusWebView.API
{
    public interface IApiService
    {
        Task<PlatformModel?> GetPlatformModelAsync(Regions region, CancellationToken cancellationToken = default);
    }
}
