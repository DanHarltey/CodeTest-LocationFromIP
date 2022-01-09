using LocationFromIP.CodeTest.Core.Interfaces;
using LocationFromIP.CodeTest.Core.Models;
using System;
using System.Threading.Tasks;

namespace LocationFromIP.CodeTest.Core.Interactors
{
    internal class LocationDetailQueryInteractor : ILocationDetailQueryInteractor
    {
        private readonly ILocationDetailQuery _locationDetailQuery;

        public LocationDetailQueryInteractor(ILocationDetailQuery locationDetailQuery) => _locationDetailQuery = locationDetailQuery;

        public Task<LocationDetailResult> GetLocationDetail(string ipAddress)
        {
            ArgumentNullException.ThrowIfNull(ipAddress);

            return _locationDetailQuery.GetLocationDetail(ipAddress);
        }
    }
}
