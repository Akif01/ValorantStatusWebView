using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using ValorantStatusWebView.API;

namespace ValorantStatusWebView.Tests
{
    [TestClass]
    public class ApiServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private ApiService _apiService;

        [TestInitialize]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://test.com/")
            };

            _apiService = new ApiService(_httpClient);
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
            var result = await _apiService.GetAsync<TestDto>("test-url");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto.Id, result.Id);
            Assert.AreEqual(expectedDto.Name, result.Name);
        }
    }

    public class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
