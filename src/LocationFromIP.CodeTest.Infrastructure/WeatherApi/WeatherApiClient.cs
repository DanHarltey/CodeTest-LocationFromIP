using LocationFromIP.CodeTest.Core.Interfaces;
using LocationFromIP.CodeTest.Core.Models;
using LocationFromIP.CodeTest.Infrastructure.WeatherApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Web;

namespace LocationFromIP.CodeTest.Infrastructure.WeatherApi
{
    internal class WeatherApiClient : ILocationDetailQuery
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly ILogger _logger;

        public WeatherApiClient(HttpClient client, IOptions<WeatherApiConfiguration> config, ILogger<WeatherApiClient> logger)
        {
            _client = client;
            _apiKey = config.Value.ApiKey;
            _logger = logger;
        }

        public async Task<LocationDetailResult> GetLocationDetail(string ipAddress)
        {
            ArgumentNullException.ThrowIfNull(ipAddress);

            var locationDetail = await RequestLocationDetail(ipAddress);

            var locationDetailResult = new LocationDetailResult
            {
                HasFoundIpAddress = locationDetail != null,
                Detail = locationDetail == null ? null : new LocationDetail()
                {
                    Ip = locationDetail.Ip,
                    Type = locationDetail.Type,
                    City = locationDetail.City,
                    Region = locationDetail.Region,
                    Country = locationDetail.CountryName,
                    CountryCode = locationDetail.CountryCode,
                    Continent = locationDetail.ContinentName,
                    IsEU = bool.Parse(locationDetail.IsEU),
                    Lat = locationDetail.Lat,
                    Lon = locationDetail.Lon
                }
            };

            return locationDetailResult;
        }

        private Task<IpLookupResponse> RequestLocationDetail(string query)
        {
            var requestUri = CreateRequestUri(query);
            var response = ExecuteRequest(requestUri, query, WeatherApiSerializerContext.Default.IpLookupResponse);

            return response;
        }

        private Uri CreateRequestUri(string query)
        {
            var queryArgs = HttpUtility.ParseQueryString(string.Empty);
            queryArgs.Add("key", _apiKey);
            queryArgs.Add("q", query);

            var uriBuilder = new UriBuilder("https://api.weatherapi.com")
            {
                Path = "/v1/ip.json",
                Query = queryArgs.ToString()
            };

            return uriBuilder.Uri;
        }

        private async Task<T> ExecuteRequest<T>(Uri requestUri, string ipAddress, JsonTypeInfo<T> typeInfo)
        {
            T responseObj = default;

            try
            {
                responseObj = await _client.GetFromJsonAsync(requestUri, typeInfo);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                _logger.LogWarning("No matching location found.", ipAddress);
            }
            catch (HttpRequestException) // None 200 result message
            {
                throw;
            }
            catch (NotSupportedException) // When content type is not valid
            {
                throw;
            }
            catch (JsonException) // Invalid JSON
            {
                throw;
            }
            catch (Polly.ExecutionRejectedException) // Polly CircuitBreaker
            {
                throw;
            }
            catch (TaskCanceledException) // Timeout
            {
                throw;
            }

            return responseObj;
        }
    }
}
