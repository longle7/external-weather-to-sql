using ExternalWeatherToSql.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExternalWeatherToSql.Controllers
{
    [ApiController]

    // The [Route("[controller]")] attribute means the route is based on the controller name.
    // For WeatherForecastController, [controller] becomes "weatherforecast", so the GET method responds to:
    // GET /weatherforecast

    // https://localhost:7077/weatherforecast -> Matched the path /weatherforecast to the WeatherForecastController.
    // Called its Get() action.
    // Serialized the returned collection to JSON via the built‑in JSON formatter (usually System.Text.Json).

    // “ASP.NET Core replaces [controller] with the controller’s class name (without ‘Controller’)
    // when the app starts, so WeatherForecastController with [Route("[controller]")] becomes /weatherforecast.”
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        // For WeatherForecastController, the controller name is WeatherForecast,
        // so the token [controller] becomes "weatherforecast" (lower‑cased by convention in the URL).
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
