namespace WeatherForecast.Service;

public interface IWeatherService
{ 
    public   Task<List<Data>?> GetWeatherByCityNameAsync(string cityName);
    public  Task<Data> GetWeatherByCityParameters(double longitude, double latitude);
    
}