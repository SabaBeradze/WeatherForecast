using System.Runtime.Serialization;

namespace WeatherForecast.Exceptions;

public class CountryNotFoundException :Exception
{
    public CountryNotFoundException()
    {
    }

    protected CountryNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CountryNotFoundException(string? message) : base(message)
    {
    }

    public CountryNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}