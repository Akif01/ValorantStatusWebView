using static ValorantStatusWebView.Components.Shared.RegionStatusCard;
using ValorantStatusWebView.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ValorantStatusWebView.API
{
    public interface IApiService
    {
        Task<TDto?> GetAsync<TDto>(string url, CancellationToken cancellationToken = default, HttpHeaders? headers = null, JsonSerializerOptions? jsonOptions = null) where TDto : class;
        Task<PlatformModel?> GetPlatformModelAsync(Regions region, CancellationToken cancellationToken = default);
    }
}
