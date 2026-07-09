var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// AddControllers() registers MVC controller support and scans for classes that inherit from ControllerBase (like WeatherForecastController).
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// using the new OpenAPI package, you don’t need AddSwaggerGen at all
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // MapOpenApi() exposes the OpenAPI JSON.
    app.MapOpenApi();

    // UseSwaggerUI serves the UI at /swagger by default and needs to know where the JSON lives (/openapi/v1.json).
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// app.MapControllers() adds route mappings based on attributes like [ApiController] and [Route("[controller]")] on those classes.
app.MapControllers();

app.Run();
