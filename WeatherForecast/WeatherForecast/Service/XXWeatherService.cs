using Microsoft.Extensions.Caching.Memory;
using WeatherForecast.Helper;

namespace WeatherForecast.Service
{
    public class XxWeatherService : IWeatherService, IWeatherAggregatorService
    {
        private readonly CountryService _countryService;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public XxWeatherService(CountryService countryService, HttpClient httpClient, IMemoryCache cache)
        {
            _countryService = countryService;
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<Data>?> GetWeatherByCityNameAsync(string cityName)
        {
            var currentDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var cacheKey = $"{cityName}_{currentDate}";

            if (_cache.TryGetValue(cacheKey, out List<Data>? cachedWeather))
            {
                return cachedWeather;
            }

            var cityObj = await _countryService.GetCountryAsync(cityName);

            var url = $"https://api.brightsky.dev/weather?lat={cityObj.Latitude}&lon={cityObj.Longitude}&date={currentDate}";
            var request = await _httpClient.GetAsync(url);

            if (!request.IsSuccessStatusCode)
            {
                return new List<Data>();
            }

            var jsonResponse = await request.Content.ReadAsStringAsync();
            var weatherResponse = jsonResponse.FromJson<Data.BrightSkyResponse>();

            if (weatherResponse == null)
            {
                return new List<Data>();
            }

            var weatherData = weatherResponse.Weather.Select(weather => new Data
            {
                City = cityObj,
                DateTime = weather.Timestamp,
                Temperature = weather.Temperature
            }).ToList();

            _cache.Set(cacheKey, weatherData, TimeSpan.FromMinutes(10));

            return weatherData;
        }

        public Task<Data> GetWeatherByCityParameters(double longitude, double latitude)
        {
            throw new NotImplementedException();
        }

        public async Task<Data> GetWeatherAsync(string city)
        {
            var temp1 = await GetWeatherByCityNameAsync(city);

            if (temp1 == null || !temp1.Any())
            {
                throw new InvalidOperationException("No weather data available.");
            }

            var temp = temp1.Average(element => element.Temperature);

            return new Data
            {
                DateTime = DateTime.Now,
                Temperature = temp,
                City = temp1.First().City
            };
        }
    }
}
