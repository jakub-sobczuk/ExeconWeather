using System.Threading.Tasks;
using Xamarin.Forms;
using ExeconWeather.Models;
using ExeconWeather.Services.Implementation;
using System.Timers;
using System.Reflection;
using System;
using ExeconWeather.Views;
using System.Windows.Input;
using ExeconWeather.Utils;

namespace ExeconWeather.ViewModels
{
    public class CityViewModel : BaseViewModel
    {
        private int _cityID;
        private WeatherModel _weatherData;
        public WeatherModel WeatherData
        {
            get => _weatherData;
            set => SetProperty(ref _weatherData, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private bool _cityFound;
        public bool CityFound
        {
            get => _cityFound;
            set => SetProperty(ref _cityFound, value);
        }

        private static Timer _timer;

        public Command RefreshCommand { get; set; }

        async Task ExecuteRefresh()
        {
            TimerCallback(this, null);
        }

        public ICommand PerformSearch => new Command<string>((string query) =>
        {
            if (query != null)
            {
                int cityID = CityNameToIDConverter.GetIDByCityName(query);

                if (cityID == -1)
                {
                    CityFound = false;
                }
                else
                {
                    CityFound = true;
                    _cityID = cityID;
                }

                TimerCallback(this, null);
            }
        });

        public CityViewModel()
        {
            Title = "Weather by city";
            RefreshCommand = new Command(async () => await ExecuteRefresh());

            _locationService = new LocationService();
            _weatherService = new WeatherService();
            _databaseService = new DatabaseService<DatabaseModel>();

            _cityFound = false;

            TimerCallback(this, null);

            _timer = new Timer(30000);
            _timer.Elapsed += TimerCallback;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        ~CityViewModel()
        {
            _timer.Enabled = false;
            _timer.Dispose();
        }

        private async void TimerCallback(object sender, ElapsedEventArgs e)
        {
            try
            {
                WeatherModel weatherModel = new WeatherModel();
                if (CityFound)
                {
                    weatherModel = _weatherService.GetWeatherForCityID(_cityID);

                    if (weatherModel.IsSuccess)
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
                        weatherModel.EmptyMessage = "Could not get weather data!";
                    }
                }
                else
                {
                    weatherModel.IsEmpty = true;
                    weatherModel.IsNotEmpty = false;

                    weatherModel.EmptyMessage = "City not found!";
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