using System.Text.Json.Serialization;

namespace WeatherForecast;

public class Data
{
    public City? City { get; set; }
    public DateTime DateTime { get; set; }
    public double Temperature{get; set; }
    public Type Type { get; set; }


    public static double TemperatureF(double temp)
    {
        return 32 + (temp / 0.5556);
        
    }
    public static double TemperatureK(double temp)
    {
        return 273.15 + temp;
    }

    public class WeatherResponseOpenMeteo
    {
        [JsonPropertyName("hourly")]
        public HourlyData Hourly { get; set; }
    }

    public class HourlyData
    {
        [JsonPropertyName("temperature_2m")]
        public List<double> Temperatures { get; set; }

        [JsonPropertyName("time")]
        public List<DateTime> Times { get; set; }
    }
    public class GeoResponse
    {
        [JsonPropertyName("results")]
        public List<City>? Results { get; set; }
    }
    public class BrightSkyResponse
    {
        [JsonPropertyName("weather")]
        public List<WeatherData> Weather { get; set; }
    }

    public class WeatherData
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }
    }
}
