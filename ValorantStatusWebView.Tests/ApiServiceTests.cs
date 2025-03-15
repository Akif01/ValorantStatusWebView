using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using ValorantStatusWebView.API;
using static ValorantStatusWebView.Components.Shared.RegionStatusCard;
using ValorantStatusWebView.DataTransferObjects;

namespace ValorantStatusWebView.Tests
{
    [TestClass]
    public class ApiServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private ApiService _apiService;
        private ConfigurationServiceStub _configServiceStub;

        [TestInitialize]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://test.com/")
            };

            _configServiceStub = new ConfigurationServiceStub();
            _apiService = new ApiService(_httpClient, _configServiceStub);
        }

        [TestMethod]
        public async Task GetAsync_ReturnsExpectedDto()
        {
            // Arrange
            var expectedDto = new TestDto { Id = 1, Name = "Test" };
            var jsonResponse = JsonSerializer.Serialize(expectedDto);
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _apiService.GetAsync<TestDto>("https://test.com/");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto.Id, result.Id);
            Assert.AreEqual(expectedDto.Name, result.Name);
        }

        [TestMethod]
        public async Task GetPlatformModelAsync_ReturnsPlatformModel_WhenResponseIsNotNull()
        {
            // Arrange
            var expectedDto = new PlatformDataDto { Id = "1", Incidents = [], Locales = [], Maintenances = [], Name = "TestName" };
            var jsonResponse = JsonSerializer.Serialize(expectedDto);
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _apiService.GetPlatformModelAsync(Regions.na);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto.Name, result.RegionName);
        }
    }

    public class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ConfigurationServiceStub : ConfigurationService
    {
        public new string ApiKey => "ApiKey";

        public ConfigurationServiceStub()
            : base(CreateMockConfiguration().Object)
        {
        }

        private static Mock<IConfiguration> CreateMockConfiguration()
        {
            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration
                .Setup(config => config["VALORANT_API_KEY"])
                .Returns("test-api-key");

            return mockConfiguration;
        }
    }

}
