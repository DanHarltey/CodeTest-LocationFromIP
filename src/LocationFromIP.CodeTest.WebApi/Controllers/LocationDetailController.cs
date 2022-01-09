using LocationFromIP.CodeTest.Core.Interfaces;
using LocationFromIP.CodeTest.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LocationFromIP.CodeTest.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationDetailController : ControllerBase
    {
        private readonly ILocationDetailQueryInteractor _queryInteractor;

        public LocationDetailController(ILocationDetailQueryInteractor queryInteractor) => _queryInteractor = queryInteractor;

        [HttpGet("{ipAddress}")]
        [ProducesResponseType(typeof(LocationDetail), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string ipAddress)
        {
            var locationDetailResult = await _queryInteractor.GetLocationDetail(ipAddress);

            if(!locationDetailResult.HasFoundIpAddress)
            {
                return NotFound();
            }

            return Ok(locationDetailResult.Detail);
        }
    }
}
