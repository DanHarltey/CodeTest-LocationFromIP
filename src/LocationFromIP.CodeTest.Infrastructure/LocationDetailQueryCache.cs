using LocationFromIP.CodeTest.Core.Interfaces;
using LocationFromIP.CodeTest.Core.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LocationFromIP.CodeTest.Infrastructure
{
    internal class LocationDetailQueryCache : ILocationDetailQuery
    {
        private readonly IDistributedCache _cache;
        private readonly ILocationDetailQuery _proxy;

        public LocationDetailQueryCache(IDistributedCache cache, ILocationDetailQuery proxy)
        {
            _cache = cache;
            _proxy = proxy;
        }

        public async Task<LocationDetailResult> GetLocationDetail(string ipAddress)
        {
            LocationDetailResult locationDetailResult;

            var cacheReuslt = await _cache.GetStringAsync(ipAddress);

            if (cacheReuslt is not null)
            {
                locationDetailResult = JsonSerializer.Deserialize<LocationDetailResult>(cacheReuslt);
            }
            else
            {
                locationDetailResult = await _proxy.GetLocationDetail(ipAddress);
                var json = JsonSerializer.Serialize(locationDetailResult);
                await _cache.SetStringAsync(ipAddress, json, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)});
            }

            return locationDetailResult;
        }
    }
}
