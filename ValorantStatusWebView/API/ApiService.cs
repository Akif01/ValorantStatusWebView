namespace ValorantStatusWebView.API
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TDto> GetAsync<TDto>(string url)
            where TDto: class
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var dto = await response.Content.ReadFromJsonAsync<TDto>();
            return dto;
        }
    }
}