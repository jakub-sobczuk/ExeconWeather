namespace ExeconWeather.Models
{
    public class DatabaseModel
    {
        public string Description { get; set; }
        public double Temp { get; set; }
        public long Pressure { get; set; }
        public long Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
