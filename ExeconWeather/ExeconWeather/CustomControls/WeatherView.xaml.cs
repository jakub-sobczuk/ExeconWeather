using ExeconWeather.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExeconWeather.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherView : ContentView
    {

        public static readonly BindableProperty WeatherDataProperty =
                BindableProperty.Create("WeatherData", typeof(WeatherModel), typeof(WeatherView), null);

        public WeatherModel WeatherData
        {
            get { return (WeatherModel)GetValue(WeatherDataProperty); }
            set { SetValue(WeatherDataProperty, value); }
        }

        public WeatherView()
        {
            InitializeComponent();
        }
    }
}