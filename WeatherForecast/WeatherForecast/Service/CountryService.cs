using Microsoft.Extensions.Caching.Memory;
using WeatherForecast.Exceptions;
using WeatherForecast.Helper;

namespace WeatherForecast.Service
{
    public class CountryService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public CountryService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<City> GetCountryAsync(string cityName)
        {
            // Check if the city data is already in cache
            if (_cache.TryGetValue(cityName, out City cachedCity))
            {
                return cachedCity;
            }

            var urlCities =
                $"https://geocoding-api.open-meteo.com/v1/search?name={cityName}&count=5&language=en&format=json";
            var request = await _httpClient.GetStringAsync(urlCities);
            var searchResult = request.FromJson<City.SearchResult>();
            var city = searchResult?.Results?.FirstOrDefault();

            if (city != null)
            {
                // Cache the city data for future requests
                _cache.Set(cityName, city, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
                return city;
            }

            throw new CountryNotFoundException("City with this name is not found!");
        }
    }
}