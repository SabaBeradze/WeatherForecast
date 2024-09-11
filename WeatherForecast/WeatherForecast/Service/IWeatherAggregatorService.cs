namespace WeatherForecast.Service;

public interface IWeatherAggregatorService
{
    public  Task<Data> GetWeatherAsync(string name);
}