# ExternalWeatherToSql

An ASP.NET Core Web API that demonstrates real-world use of `async`/`await` by calling an external weather API and saving results to SQL Server using Entity Framework Core.

## What this project shows

- Async controller actions (`Task<IActionResult>`) with `await` inside.
- Non-blocking HTTP calls using `HttpClient.GetStringAsync(...)`.
- Asynchronous database writes with `await _dbContext.SaveChangesAsync()`.
- EF Core `DbContext` and `DbSet` mapping (`WeatherDbContext` and `WeatherReading`).
- Swagger/OpenAPI UI for testing endpoints.

## Endpoints

- `GET /WeatherForecast`  
  Default ASP.NET Core example endpoint that returns sample weather data.

- `GET /ExternalWeather/sync?city=Boston`  
  Calls an external weather API (API key loaded from configuration/User Secrets), creates a `WeatherReading` entity, and saves it to SQL Server using `SaveChangesAsync`. Returns the saved entity as JSON.

- `GET /ExternalWeather/latest?city=Boston`  
  Reads the latest `WeatherReading` for the specified city from SQL Server using EF Core and returns it as JSON.

## Tech Stack

- .NET 10 / ASP.NET Core Web API
- C#
- Entity Framework Core (SQL Server)
- SQL Server (`WeatherDb` database)
- Swagger / OpenAPI

## Async/await concept demonstrated

This project is built to answer common async/await interview questions in a practical way:

- `HttpClient.GetStringAsync` returns a `Task<string>`; `await` suspends the method until that task completes and unwraps the `string` result.
- `SaveChangesAsync` returns a `Task<int>`; `await` pauses the method while EF Core sends SQL to the database, without blocking the request thread.

Under load, using `async`/`await` here allows the Web API to handle more concurrent requests with fewer threads, because the server threads are free while waiting on HTTP and database I/O.

## Running the project

1. Clone the repo:

   ```bash
   git clone https://github.com/longle7/external-weather-to-sql.git
   cd external-weather-to-sql
   ```

2. Configure the database:

   - Update the SQL Server connection string in `appsettings.json` under `ConnectionStrings:WeatherDb`.
   - Run EF Core migrations:

     ```bash
     dotnet ef database update
     ```

3. Configure the weather API key:

   - Use ASP.NET Core User Secrets (recommended for local dev):
     ```bash
     dotnet user-secrets init
     dotnet user-secrets set "WeatherApi:ApiKey" "YOUR_REAL_API_KEY"
     ```

4. Run the API:

   ```bash
   dotnet run
   ```

5. Open Swagger UI at:

   ```text
   https://localhost:{port}/swagger
   ```

   Test `ExternalWeather/sync` and `ExternalWeather/latest` from the browser.

## Why this exists

This project was created to practice and demonstrate:

- Async/await in C# in a real web API scenario.
- How ASP.NET Core uses async controller actions to improve scalability.
- EF Core + SQL Server integration with asynchronous database operations.

It is intended as a portfolio piece and an interview reference for explaining `async`/`await`, `HttpClient`, and EF Core in a concrete, code-backed way.