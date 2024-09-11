namespace WeatherForecast.Service;

public class WeatherAggregatorService
{
    private readonly IEnumerable<IWeatherAggregatorService> _services;

    public WeatherAggregatorService(IEnumerable<IWeatherAggregatorService> services)
    {
        _services = services;
    }

    public async Task<Data> GetAverageAsync(string cityName, Type type)
    {
        var tasks = _services.Select(serv => serv.GetWeatherAsync(cityName));
        var list = await Task.WhenAll(tasks);

        var averageTemperature = list.Average(i => i.Temperature);
        var city = list.First().City;
        var dateTime = DateTime.Now;

        var data = new Data
        {
            Temperature = averageTemperature,
            City = city,
            DateTime = dateTime
        };

        return await GetSpecificTypeData(data, type);
    }

    private Task<Data> GetSpecificTypeData(Data data, Type type)
    {
        data.Type = type;
        switch (type)
        {
            case Type.Fahrenheit:
                data.Temperature = Data.TemperatureF(data.Temperature);
                break;
            case Type.Kelvin:
                data.Temperature = Data.TemperatureK(data.Temperature);
                break;
            case Type.Celsius:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return Task.FromResult(data);
    }
}
