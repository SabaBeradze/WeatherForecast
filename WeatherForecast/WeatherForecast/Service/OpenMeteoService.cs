using Microsoft.Extensions.Caching.Memory;
using WeatherForecast.Helper;

namespace WeatherForecast.Service
{
    public class OpenMeteoService : IWeatherService, IWeatherAggregatorService
    {
        private readonly CountryService _countryService;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public OpenMeteoService(CountryService countryService, HttpClient httpClient, IMemoryCache cache)
        {
            _countryService = countryService;
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<Data>?> GetWeatherByCityNameAsync(string cityName)
        {
            if (_cache.TryGetValue($"weather_{cityName}", out List<Data> cachedWeather))
            {
                return cachedWeather;
            }

            var city = await _countryService.GetCountryAsync(cityName);

            var url = $"https://api.open-meteo.com/v1/forecast?latitude={city.Latitude}&longitude={city.Longitude}&hourly=temperature_2m";
            var request = await _httpClient.GetAsync(url);

            if (!request.IsSuccessStatusCode) return null;

            var jsonResponse = await request.Content.ReadAsStringAsync();
            var weatherResponse = jsonResponse.FromJson<Data.WeatherResponseOpenMeteo>();

            var weatherData = weatherResponse?.Hourly.Times.Select((t, i) => new Data
            {
                City = city,
                DateTime = t,
                Temperature = weatherResponse.Hourly.Temperatures[i]
            }).ToList();

            if (weatherData != null)
            {
                _cache.Set($"weather_{cityName}", weatherData, TimeSpan.FromMinutes(10));
            }

            return weatherData;
        }

        public Task<Data> GetWeatherByCityParameters(double longitude, double latitude)
        {
            throw new NotImplementedException();
        }

        public async Task<Data> GetWeatherAsync(string city)
        {
            var listOfData = await GetWeatherByCityNameAsync(city);
            if (listOfData == null || !listOfData.Any())
            {
                throw new InvalidOperationException("No weather data available.");
            }

            var temp = listOfData.Average(element => element.Temperature);

            return new Data
            {
                DateTime = DateTime.Now,
                Temperature = temp,
                City = listOfData.First().City
            };
        }
    }
}
