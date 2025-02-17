namespace ValorantStatusWebView
{
    using Microsoft.Extensions.Configuration;

    public class ConfigurationService
    {
        public string ApiKey { get; }

        public ConfigurationService(IConfiguration configuration)
        {
            ApiKey = configuration["VALORANT_API_KEY"];

            if (string.IsNullOrEmpty(ApiKey))
                throw new Exception("ApiKey is not configured in environment variables!");
        }
    }
}
