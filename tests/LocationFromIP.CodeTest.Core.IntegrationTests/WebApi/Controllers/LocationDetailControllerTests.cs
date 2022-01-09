using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LocationFromIP.CodeTest.Core.IntegrationTests.WebApi.Controllers
{
    public class LocationDetailControllerTests : UsesWebApplicationFactory
    {
        public LocationDetailControllerTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public async Task WhenIpAddressFoundThenReturnDetail()
        {
            // Arrange
            using var client = CreateClient();

            // Act
            var locationDetail = await client.GetFromJsonAsync<LocationDetail>("/api/LocationDetail/8.8.8.8");

            // Assert
            Assert.NotNull(locationDetail);
            Assert.Equal("8.8.8.8", locationDetail.Ip);
            Assert.Equal("ipv4", locationDetail.Type);
            Assert.Null(locationDetail.City);
            Assert.Equal("Kansas", locationDetail.Region);
            Assert.Equal("United States", locationDetail.Country);
            Assert.Equal("US", locationDetail.CountryCode);
            Assert.Equal("North America", locationDetail.Continent);
            Assert.Equal(37.751, locationDetail.Lat);
            Assert.Equal(-97.822, locationDetail.Lon);
        }

        [Fact]
        public async Task WhenNoIpAddressProvidedThenReturn404()
        {
            // Arrange
            using var client = CreateClient();

            // Act, Assert
            var httpRequestException = await Assert.ThrowsAsync<HttpRequestException>(
                () => client.GetFromJsonAsync<LocationDetail>("/api/LocationDetail/"));

            Assert.Equal(HttpStatusCode.NotFound, httpRequestException.StatusCode);
        }

        [Fact]
        public async Task WhenIpAddressNotFoundThenReturn404()
        {
            // Arrange
            using var client = CreateClient();

            // Act
            var httpRequestException = await Assert.ThrowsAsync<HttpRequestException>(
                () => client.GetFromJsonAsync<LocationDetail>("/api/LocationDetail/192.168.0.20"));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, httpRequestException.StatusCode);
        }

        private class LocationDetail
        {
            public string Ip { get; init; }

            public string Type { get; init; }

            public string? City { get; init; }

            public string Region { get; init; }

            public string Country { get; init; }
            public string CountryCode { get; init; }

            public string Continent { get; init; }

            public bool IsEU { get; init; }

            public double Lat { get; init; }

            public double Lon { get; init; }
        }
    }
}