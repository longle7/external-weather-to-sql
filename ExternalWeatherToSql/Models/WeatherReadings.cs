namespace ExternalWeatherToSql.Models
{
    public class WeatherReading
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public double TemperatureC { get; set; }
        public string RawJson { get; set; } = string.Empty;
    }
}