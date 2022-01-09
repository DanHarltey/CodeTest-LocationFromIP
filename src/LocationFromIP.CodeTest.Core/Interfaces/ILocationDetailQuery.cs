using LocationFromIP.CodeTest.Core.Models;
using System.Threading.Tasks;

namespace LocationFromIP.CodeTest.Core.Interfaces
{
    public interface ILocationDetailQuery
    {
        Task<LocationDetailResult> GetLocationDetail(string ipAddress);
    }
}
