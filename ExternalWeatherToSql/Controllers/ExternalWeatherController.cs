using System.Net.Http;
using System.Threading.Tasks;
using ExternalWeatherToSql.Data;
using ExternalWeatherToSql.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExternalWeatherToSql.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class ExternalWeatherController : ControllerBase
    {
        private readonly WeatherDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;


        // “SyncWeather is an async controller action. It awaits HttpClient.GetStringAsync to
        // fetch external weather JSON without blocking the request thread. Then it creates a WeatherReading
        // entity, adds it to the WeatherReadings DbSet, and awaits SaveChangesAsync to persist it to SQL Server.
        // Finally it returns Ok(reading), which sends a 200 response with the saved reading serialized as JSON.”
        public ExternalWeatherController(WeatherDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        // GET /ExternalWeather/sync?city=Boston
        [HttpGet("sync")]

        // async is a "modifier"
        // SyncWather is a async controller action
        public async Task<IActionResult> SyncWeather(string city = "Boston")
        {
            // 1. Call external API asynchronously
            // Sends an HTTP GET request and asynchronously waits for the response body as a string.
            var apiKey = _configuration["WeatherApi:ApiKey"];
            var url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}";

            // Exceptions will be bubbled up and handled by ASP.NET
            // You only throw yourself when you want to signal an error condition (e.g., bad input).
            // 500 error by default using the ASP.NET handling
            try
            {
                // While the HTTP call is in progress, the thread is free — but your method does not continue to the next line until the await completes.
                // Without await, you get a Task<string> (a promise of future data), not the JSON itself.
                // var json1 = _httpClient.GetStringAsync(url);

                // await waits for the task to complete then extracts the string result from the task
                // await is an operator
                // The request thread is freed, and the runtime can use it to process other requests or methods.
                var json = await _httpClient.GetStringAsync(url);

                // 2. Create a WeatherReading entity
                var reading = new WeatherReading
                {
                    City = city,
                    Timestamp = DateTime.UtcNow,
                    TemperatureC = 0, // we can parse real temp later
                    RawJson = json
                };

                // 3. Add to DbSet and save changes asynchronously
                // It doesn’t hit the DB yet; it’s just in the change tracker.
                // await pauses the method until the DB write completes.
                _dbContext.WeatherReadings.Add(reading);

                // Asynchronously writes pending changes (WeatherReadings.Add) to SQL Server.
                await _dbContext.SaveChangesAsync();

                // “Ok(...) is a convenience method that creates a 200 response with the given object serialized to JSON.”
                return Ok(reading);
            }
            catch (DbUpdateException dbEx)
            {
                // EF Core/SQL-specific error
                return StatusCode(500, $"Database error while saving weather: {dbEx.Message}");
            }
            catch (HttpRequestException httpEx)
            {
                // HTTP call failed
                return StatusCode(502, $"Weather API error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // fallback
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }

        }

        // GET /ExternalWeather/latest?city=Boston
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest(string city = "Boston")
        {
            var latest = await _dbContext.WeatherReadings
                .Where(r => r.City == city)
                .OrderByDescending(r => r.Timestamp)
                .FirstOrDefaultAsync();

            if (latest == null)
                return NotFound();

            return Ok(latest);
        }
    }
}