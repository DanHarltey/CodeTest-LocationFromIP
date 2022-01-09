using System.Text.Json.Serialization;

namespace LocationFromIP.CodeTest.Infrastructure.WeatherApi.Models
{
    [JsonSerializable(typeof(IpLookupResponse))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    internal partial class WeatherApiSerializerContext : JsonSerializerContext
    {
    }
}
