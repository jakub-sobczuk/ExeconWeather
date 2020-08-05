using ExeconWeather.Models;

namespace ExeconWeather.Services.Interface
{
    public interface IWeatherService
    {
        WeatherModel GetWeatherForCityID(int id);

        WeatherModel GetWeatherForLocation(double lat, double lon);

        string GetWeatherIconPath(string iconID);
    }
}
