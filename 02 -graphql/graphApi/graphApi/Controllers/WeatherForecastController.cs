using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace graphApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : GraphController
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [QueryRoot]
    public WeatherForecast Get()
    {
        return new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
    }

    [MutationRoot]
    public WeatherForecast Post(WeatherForecast weatherForecast)
    {
        return weatherForecast;
    }
}