using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using ValorantStatusWebView.DataTransferObjects;
using ValorantStatusWebView.Models;
using static ValorantStatusWebView.Components.Shared.RegionStatusCard;

namespace ValorantStatusWebView.API
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        private readonly string _apiKey;
        private readonly JsonSerializerOptions _defaultJsonOptions;
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);

        public ApiService(
            HttpClient httpClient,
            ConfigurationService configService,
            ILogger<ApiService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _apiKey = configService.ApiKey ?? throw new InvalidOperationException("API key is not configured");

            // Set up default JSON options once
            _defaultJsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private string GetValorantStatusUrl(Regions region)
        {
            return $"https://{region}.api.riotgames.com/val/status/v1/platform-data";
        }

        public async Task<PlatformModel?> GetPlatformModelAsync(Regions region, CancellationToken cancellationToken = default)
        {
            var url = GetValorantStatusUrl(region);
            _logger.LogInformation("Fetching platform status for region '{Region}'", region);

            var headers = new HttpRequestMessage().Headers;
            headers.Add("X-Riot-Token", _apiKey);
            var response = await GetDtoAsync<PlatformDataDto>(
                url,
                headers,
                cancellationToken: cancellationToken);

            return response is not null ? new PlatformModel(response) : null;
        }

        public async Task<TDto?> GetDtoAsync<TDto>(
            string url,
            HttpHeaders? headers = null,
            JsonSerializerOptions? jsonOptions = null,
            CancellationToken cancellationToken = default)
            where TDto : class
        {
            try
            {
                _logger.LogInformation("Sending GET request to {Url}", url);
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Apply headers if provided
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                // Configure request timeout
                using var timeoutCts = new CancellationTokenSource(_defaultTimeout);
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    timeoutCts.Token,
                    cancellationToken
                );

                var response = await _httpClient.SendAsync(request, linkedCts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(linkedCts.Token);
                    _logger.LogError("HTTP request failed with status {StatusCode}: {ErrorContent}",
                        response.StatusCode, errorContent);
                    response.EnsureSuccessStatusCode();
                }

                return await response.Content.ReadFromJsonAsync<TDto>(
                    jsonOptions ?? _defaultJsonOptions,
                    linkedCts.Token
                );
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for URL: '{Url}' with status code: {StatusCode}",
                    url, ex.StatusCode);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error for URL: '{Url}'. Path: {Path}, LineNumber: {LineNumber}",
                    url, ex.Path, ex.LineNumber);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Request to '{Url}' was {Reason}",
                    url, cancellationToken.IsCancellationRequested ? "cancelled" : "timed out");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending GET request to '{Url}'", url);
            }

            return null;
        }
    }
}