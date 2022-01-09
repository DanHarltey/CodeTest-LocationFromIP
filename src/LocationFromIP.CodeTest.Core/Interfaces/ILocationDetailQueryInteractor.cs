using LocationFromIP.CodeTest.Core.Models;
using System.Threading.Tasks;

namespace LocationFromIP.CodeTest.Core.Interfaces
{
    public interface ILocationDetailQueryInteractor 
    {
        Task<LocationDetailResult> GetLocationDetail(string ipAddress);
    }
}