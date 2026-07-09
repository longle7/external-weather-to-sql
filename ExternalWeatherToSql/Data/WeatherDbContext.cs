using ExternalWeatherToSql.Models;
using Microsoft.EntityFrameworkCore;

namespace ExternalWeatherToSql.Data
{
    // So WeatherDbContext is “your app’s view of the WeatherDb database”.
    public class WeatherDbContext : DbContext
    {
        // This constructor takes configuration options (e.g., which provider, connection string).
        // In Program.cs, you pass these options when you call AddDbContext with UseSqlServer(connectionString).
        // EF Core uses those options to connect to SQL Server.

        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options)
        {
        }

        // DbSet<T> represents a table (or collection) of T entities in the database.
        // WeatherReadings is “the WeatherReadings table in WeatherDb”, giving you LINQ access: WeatherReadings.Add(...), WeatherReadings.Where(...), etc.
        // The get/set is just the property; EF Core wires it to the underlying table at runtime based on conventions and migrations.

        // "WeatherReadings is the DbSet that represents the WeatherReadings table, so I use it to query and save WeatherReading entities."
        public DbSet<WeatherReading> WeatherReadings { get; set; } = null!;
    }
}