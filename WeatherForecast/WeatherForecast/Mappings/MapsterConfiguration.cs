using Mapster;

namespace WeatherForecast.Mappings;

public static class MapsterConfiguration
{
    public static void RegisterMaps(this IServiceCollection services)
    {
        TypeAdapterConfig<Data, Data>.NewConfig();
    }
}