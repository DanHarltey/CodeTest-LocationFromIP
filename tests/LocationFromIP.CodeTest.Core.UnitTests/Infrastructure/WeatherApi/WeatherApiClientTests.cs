using LocationFromIP.CodeTest.Infrastructure.WeatherApi;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LocationFromIP.CodeTest.Core.UnitTests.Infrastructure.WeatherApi
{
    public class WeatherApiClientTests
    {
        private readonly IOptions<WeatherApiConfiguration> _config;

        public WeatherApiClientTests() => _config = Options.Create(new WeatherApiConfiguration());

        const string SuccessJson = @"{
	            ""ip"": ""236.236.236.236"",
	            ""type"": ""ipv4"",
	            ""continent_code"": ""EU"",
	            ""continent_name"": ""Europe"",
	            ""country_code"": ""GB"",
	            ""country_name"": ""United Kingdom"",
	            ""is_eu"": ""false"",
	            ""geoname_id"": 2638077,
	            ""city"": ""Leeds"",
	            ""region"": ""Yorkshire"",
	            ""lat"": 53.8008,
	            ""lon"": 1.5491,
	            ""tz_id"": ""Europe/London"",
	            ""localtime_epoch"": 1641735597,
	            ""localtime"": ""2022-01-09 13:39""
            }";

        [Fact]
        public async void GetLocationDetailReturnsSuccess()
        {
            // Arrange
            var mockHttp = CreateHttpClient(SuccessJson);
            var client = new WeatherApiClient(mockHttp, _config, NullLogger<WeatherApiClient>.Instance);

            // Act
            var locationDetail = await client.GetLocationDetail("236.236.236.236");

            // Assert
            Assert.NotNull(locationDetail);
            Assert.True(locationDetail.HasFoundIpAddress);
            Assert.Equal("236.236.236.236", locationDetail.Detail.Ip);
            Assert.Equal("ipv4", locationDetail.Detail.Type);
            Assert.Equal("Europe", locationDetail.Detail.Continent);
            Assert.Equal("GB", locationDetail.Detail.CountryCode);
            Assert.Equal("United Kingdom", locationDetail.Detail.Country);
            Assert.False(locationDetail.Detail.IsEU);
            Assert.Equal("Leeds", locationDetail.Detail.City);
            Assert.Equal("Yorkshire", locationDetail.Detail.Region);
            Assert.Equal(53.8008, locationDetail.Detail.Lat);
            Assert.Equal(1.5491, locationDetail.Detail.Lon);
        }

        [Fact]
        public async void WhenIpAddressNotFoundThenReturnNotFound()
        {
            // Arrange
            var mockHttp = CreateHttpClient(string.Empty, HttpStatusCode.BadRequest);
            var client = new WeatherApiClient(mockHttp, _config, NullLogger<WeatherApiClient>.Instance);

            // Act
            var locationDetail = await client.GetLocationDetail("BadIpAddress");

            // Assert
            Assert.NotNull(locationDetail);
            Assert.False(locationDetail.HasFoundIpAddress);
            Assert.Null(locationDetail.Detail);
        }

        private static HttpClient CreateHttpClient(string content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            var weatherResponse = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content),
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.AbsolutePath.Contains("/v1/ip.json")),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(weatherResponse);

            var httpClient = new HttpClient(handlerMock.Object);
            return httpClient;
        }
    }
}
