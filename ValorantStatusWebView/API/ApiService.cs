using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ValorantStatusWebView.DataTransferObjects;
using ValorantStatusWebView.Models;
using static ValorantStatusWebView.Components.Shared.RegionStatusCard;

namespace ValorantStatusWebView.API
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ConfigurationService _configService;
        private readonly ILogger<ApiService> _logger;

        public ApiService(
            HttpClient httpClient,
            ConfigurationService configService,
            ILogger<ApiService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private string GetURL(Regions region)
        {
            return $"https://{region}.api.riotgames.com/val/status/v1/platform-data";
        }

        public async Task<PlatformModel?> GetPlatformModelAsync(Regions region, CancellationToken cancellationToken = default)
        {
            var url = GetURL(region);
            var headers = new HttpRequestMessage().Headers;
            headers.Add("X-Riot-Token", _configService.ApiKey);

            var response = await GetAsync<PlatformDataDto>(url, cancellationToken, headers);

            return response is not null ? new PlatformModel(response) : null;
        }

        public async Task<TDto?> GetAsync<TDto>(
            string url,
            CancellationToken cancellationToken = default,
            HttpHeaders? headers = null,
            JsonSerializerOptions? jsonOptions = null)
            where TDto : class
        {
            try
            {
                _logger.LogInformation("Sending GET request to {Url}", url);

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Add headers if provided
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                // Configure request to timeout after a reasonable period
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    timeoutCts.Token,
                    cancellationToken
                );

                var response = await _httpClient.SendAsync(request, linkedCts.Token);

                response.EnsureSuccessStatusCode();

                jsonOptions ??= new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return await response.Content.ReadFromJsonAsync<TDto>(
                    jsonOptions,
                    linkedCts.Token
                );
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for URL: {Url}", url);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error for URL: {Url}", url);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Request to {Url} was cancelled or timed out", url);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unexpected error while sending GET request to {Url}", url);
            }

            return null;
        }
    }
}