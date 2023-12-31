using Serilog;

// Setup a Serilog logger that is just used in Program.cs.
// Usually if there are problems when your app starts up, your application logger won't catch them because it's not yet running.
// That's why it makes sense to configure a separate logger specifically to wrap your application and its startup.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Information("Starting Up");

// Place all logic in a try catch to log any potential exception to the Serilog sink.
try {

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    Log.Logger.Information("Number of services {count}", builder.Services.Count);

    builder.Services.Configure<WeatherOptions>(
        builder.Configuration.GetSection(WeatherOptions.ConfigSectionName));
    var weatherOptions = WeatherOptions.LoadAndValidate(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    app.MapGet("/weatherforecast", () =>
    {
        var forecast =  Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Application terminated unexpectedely");
}
finally
{
    Log.CloseAndFlush();
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
