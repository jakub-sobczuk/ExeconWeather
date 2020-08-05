using ExeconWeather.Models;
using ExeconWeather.Services.Interface;
using System;
using System.Collections.Generic;
using System.Web;

namespace ExeconWeather.Services.Implementation
{
    public class WeatherService : IWeatherService
    {
        private const string ENDPOINT = "http://api.openweathermap.org/data/2.5/weather";

        private const string API_KEY = "9d31ee8ab88cf7721654ce9fcd9840a3";
        private const string LAT_PARAM = "lat";
        private const string LON_PARAM = "lon";
        private const string API_KEY_PARAM = "appid";
        private const string ID_PARAM = "id";

        private IWebService _webService;

        public WeatherService()
        {
            _webService = new WebService();
        }

        private string BuildUri(List<KeyValuePair<string, string>> parameters)
        {
            string endpoint = ENDPOINT;
            var uriBuilder = new UriBuilder(endpoint);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var item in parameters)
            {
                query[item.Key] = item.Value;
            }

            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        public WeatherModel GetWeatherForCityID(int id)
        {
            WeatherModel weatherModel = new WeatherModel();
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(ID_PARAM,id.ToString()),
                new KeyValuePair<string, string>(API_KEY_PARAM, API_KEY)
            };

            try
            {
                string response = _webService.GetResponseText(BuildUri(parameters));
                weatherModel = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(response);
            }
            catch (Exception ex)
            {
                weatherModel.IsSuccess = false;
                weatherModel.ExMessage = ex.Message;
            }

            weatherModel.IsSuccess = true;
            return weatherModel;
        }

        public WeatherModel GetWeatherForLocation(double lat, double lon)
        {
            WeatherModel weatherModel = new WeatherModel();
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(LAT_PARAM,lat.ToString()),
                new KeyValuePair<string, string>(LON_PARAM,lon.ToString()),
                new KeyValuePair<string, string>(API_KEY_PARAM, API_KEY)
            };

            try
            {
                string response = _webService.GetResponseText(BuildUri(parameters));
                weatherModel = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(response);
            }

            catch (Exception ex)
            {
                weatherModel.IsSuccess = false;
                weatherModel.ExMessage = ex.Message;
            }

            weatherModel.IsSuccess = true;
            return weatherModel;
        }

        public string GetWeatherIconPath(string iconID)
        {
            return iconID + "@2x.png";
        }
    }
}
