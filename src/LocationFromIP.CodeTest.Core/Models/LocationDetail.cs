namespace LocationFromIP.CodeTest.Core.Models
{
    public class LocationDetail
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
