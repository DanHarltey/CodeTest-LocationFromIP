using System.Text.Json.Serialization;

namespace LocationFromIP.CodeTest.Infrastructure.WeatherApi.Models
{
    internal class IpLookupResponse
    {
        public string Ip { get; set; }

        public string Type { get; set; }

        [JsonPropertyName("continent_code")]
        public string ContinentCode { get; set; }

        [JsonPropertyName("continent_name")]
        public string ContinentName { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        [JsonPropertyName("is_eu")]
        public string IsEU { get; set; }

        [JsonPropertyName("geoname_id")]
        public int? GeonameId { get; set; }

        public string? City { get; set; }

        public string Region { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        [JsonPropertyName("tz_id")]
        public string TimeZoneId { get; set; }

        [JsonPropertyName("localtime_epoch")]
        public long LocalTimeEpoch { get; set; }

        [JsonPropertyName("localtime")]
        public string LocalTime { get; set; }
    }
}
