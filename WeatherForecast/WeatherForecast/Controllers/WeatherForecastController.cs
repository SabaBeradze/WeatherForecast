using Mapster;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Helper;
using WeatherForecast.Service;

namespace WeatherForecast.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WeatherAggregatorService _service;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherAggregatorService service)
    {
        _logger = logger;
        _service = service;
    }

    
    [HttpGet("{cityName}")]
    public async Task<IActionResult> GetWeatherDataAsync(string cityName,Type type)
    {  
        var data = await _service.GetAverageAsync(cityName,type);
        var dataDto = data.Adapt<DataDTO>();

        return Ok(dataDto);
    }
}