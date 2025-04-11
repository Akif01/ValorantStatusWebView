using static ValorantStatusWebView.Components.Shared.RegionStatusCard;
using ValorantStatusWebView.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ValorantStatusWebView.API
{
    public interface IApiService
    {
        Task<TDto?> GetDtoAsync<TDto>(string url, HttpHeaders? headers = null, JsonSerializerOptions? jsonOptions = null, CancellationToken cancellationToken = default) where TDto : class;
        Task<PlatformModel?> GetPlatformModelAsync(Regions region, CancellationToken cancellationToken = default);
    }
}
