
namespace WeatherForecast;

public class City
{
    public string? Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public override string ToString()
    {
        return Name;
    }
    public class SearchResult
    {
        public List<City>? Results { get; set; }
    }

}
