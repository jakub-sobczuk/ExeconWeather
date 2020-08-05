namespace ExeconWeather.Models
{
    public class LocationModel
    {
        public bool IsSuccess { get; set; }
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string ExMessage { get; set; }
    }
}
