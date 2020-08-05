using System.Threading.Tasks;
using Xamarin.Forms;
using ExeconWeather.Models;
using ExeconWeather.Services.Implementation;
using System.Timers;
using System.Reflection;
using System;
using ExeconWeather.Views;
using Plugin.Geolocator;

namespace ExeconWeather.ViewModels
{
    public class LocationViewModel : BaseViewModel
    {
        private WeatherModel _weatherData;
        public WeatherModel WeatherData
        {
            get => _weatherData;
            set => SetProperty(ref _weatherData, value);
        }

        private bool _locationEnabled;
        public bool LocationEnabled
        {
            get => _locationEnabled;
            set => SetProperty(ref _locationEnabled, value);
        }

        private static Timer _timer;
        private static Timer _geolocatorTimer;

        public Command RefreshCommand { get; set; }

        async Task ExecuteRefresh()
        {
            TimerCallback(this, null);
        }

        public LocationViewModel()
        {
            Title = "Weather by location";
            RefreshCommand = new Command(async () => await ExecuteRefresh());

            _locationService = new LocationService();
            _weatherService = new WeatherService();
            _databaseService = new DatabaseService<DatabaseModel>();

            TimerCallback(this, null);

            _timer = new Timer(30000);
            _timer.Elapsed += TimerCallback;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            _geolocatorTimer = new Timer(2000);
            _geolocatorTimer.Elapsed += GeolocationEnabledCallback;
            _geolocatorTimer.AutoReset = true;
            _geolocatorTimer.Enabled = true;
        }

        ~LocationViewModel()
        {
            _timer.Enabled = false;
            _geolocatorTimer.Enabled = false;
            _timer.Dispose();
        }

        private void GeolocationEnabledCallback(object sender, ElapsedEventArgs e)
        {
            LocationEnabled = CrossGeolocator.Current.IsGeolocationEnabled;
        }

        private async void TimerCallback(object sender, ElapsedEventArgs e)
        {
            try
            {
                WeatherModel weatherModel = new WeatherModel();
                LocationModel location = new LocationModel();
                if (LocationEnabled)
                {
                    location = await _locationService.GetCurrentLocation();
                    if (!location.IsSuccess)
                    {
                        location = await _locationService.GetLastKnownLocation();
                    }
                }

                weatherModel = _weatherService.GetWeatherForLocation(location.Latitude, location.Longitude);

                if (weatherModel.IsSuccess && location.IsSuccess && LocationEnabled)
                {
                    weatherModel.IsEmpty = false;
                    weatherModel.IsNotEmpty = true;

                    weatherModel.WeatherSingle = weatherModel.Weather[0];
                    weatherModel.IconPath = ImageSource.FromResource("ExeconWeather.Resources." + _weatherService.GetWeatherIconPath(weatherModel.WeatherSingle.Icon),
                            typeof(LocationViewModel).GetTypeInfo().Assembly);

                    await _databaseService.CreateTable();

                    var databaseModel = new DatabaseModel()
                    {
                        Description = weatherModel.WeatherSingle.Description,
                        Temp = weatherModel.Main.Temp,
                        Pressure = weatherModel.Main.Pressure,
                        Humidity = weatherModel.Main.Humidity,
                        WindSpeed = weatherModel.Wind.Speed,
                        Lat = weatherModel.Coord.Lat,
                        Lon = weatherModel.Coord.Lon,
                    };

                    await _databaseService.Insert(databaseModel);
                    //var asd = await _databaseService.Get(); //-------------------------------------------------------------Database test
                }
                else
                {
                    weatherModel.IsEmpty = true;
                    weatherModel.IsNotEmpty = false;

                    if (!weatherModel.IsSuccess)
                    {
                        weatherModel.EmptyMessage = "Could not get weather data!";
                    }
                    if (!location.IsSuccess)
                    {
                        weatherModel.EmptyMessage = "Could not get location data!";
                    }
                    if (!LocationEnabled)
                    {
                        weatherModel.EmptyMessage = "Geolocation is disabled!";
                    }
                }

                WeatherData = weatherModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception occured: " + ex.Message);
            }
        }
    }
}