using ExeconWeather.Models;
using System.Threading.Tasks;

namespace ExeconWeather.Services.Interface
{
    public interface ILocationService
    {
        Task<LocationModel> GetCurrentLocation();

        Task<LocationModel> GetLastKnownLocation();
    }
}
