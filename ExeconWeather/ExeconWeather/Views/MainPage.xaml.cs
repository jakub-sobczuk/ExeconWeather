using System.ComponentModel;
using Xam = Xamarin.Forms;
using System.Drawing;
using Xamarin.Essentials;

namespace ExeconWeather.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : Xam.TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

            BarBackgroundColor = ColorConverters.FromHex("#ed8756");
            BarTextColor = Color.White;
            SelectedTabColor = Color.White;
            UnselectedTabColor = Color.Gray;
        }
    }
}