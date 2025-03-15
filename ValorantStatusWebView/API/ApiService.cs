using ValorantStatusWebView.DataTransferObjects;
using ValorantStatusWebView.Models;
using static ValorantStatusWebView.Components.Shared.RegionStatusCard;

namespace ValorantStatusWebView.API
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ConfigurationService _configService;

        public ApiService(HttpClient httpClient, ConfigurationService configService)
        {
            _httpClient = httpClient;
            _configService = configService;
        }

        private string GetBaseUrl(Regions region)
        {
            return $"https://{region}.api.riotgames.com/val/status/v1/platform-data?api_key={_configService.ApiKey}";
        }

        public async Task<PlatformModel?> GetPlatformModelAsync(Regions region)
        {
            var url = GetBaseUrl(region);
            var response = await GetAsync<PlatformDataDto>(url);

            return response is not null ? new PlatformModel(response) : null;
        }

        public async Task<TDto?> GetAsync<TDto>(string url)
            where TDto : class
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TDto>();
        }
    }
}
