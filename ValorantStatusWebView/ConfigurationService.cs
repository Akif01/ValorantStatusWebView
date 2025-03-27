using Microsoft.Extensions.Configuration;
using System.IO;

namespace ValorantStatusWebView
{
    public class ConfigurationService
    {
        private readonly ILogger<ConfigurationService> _logger;
        private readonly IHostEnvironment _environment;
        public string ApiKey { get; }

        public ConfigurationService(
            IConfiguration configuration,
            ILogger<ConfigurationService> logger,
            IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;

            ApiKey = configuration["valorant_api_key"] ?? ReadKeyFromFile("valorant_api_key.txt");

            if (string.IsNullOrEmpty(ApiKey))
            {
                if (_environment.IsDevelopment())
                {
                    CreateDummyKeyFile("valorant_api_key.txt");
                    ApiKey = "dummy_api_key_12345"; // Dummy key
                    _logger.LogWarning("Using dummy API key for development");
                }
                else
                {
                    throw new InvalidOperationException(
                        "API key not configured. In production, use Docker secrets.\n" +
                        "In development, create valorant_api_key.txt or it will use a dummy key");
                }
            }

            _logger.LogInformation("API key initialized (last 4 chars: {LastChars})",
                ApiKey.Length > 4 ? ApiKey[^4..] : "****");
        }

        private static string? ReadKeyFromFile(string filePath)
        {
            try
            {
                return File.Exists(filePath)
                    ? File.ReadAllText(filePath).Trim()
                    : null;
            }
            catch
            {
                return null;
            }
        }

        private static void CreateDummyKeyFile(string path)
        {
            try
            {
                var dummyContent = "dummy_api_key_12345";

                File.WriteAllText(path, dummyContent);
            }
            catch (Exception ex)
            {
                // If file creation fails, the null check will handle it
            }
        }
    }
}