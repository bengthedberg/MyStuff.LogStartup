# Log at StartUp

Use Serilog as a standalone logger for ASP.NET Core startup process.

> Setup a Serilog logger that is just used in Program.cs

Usually if there are problems when your app starts up, your application logger won't catch them because it's not yet running.
That's why it makes sense to configure a separate logger specifically to wrap your application and its startup.

Add following Nuget packages:

* Serilog.AspNetCore
* Serilog.Settings.Configuration
* Serilog.Sinks.Console (or whatever other sinks you want to use)

Add the following to the beginning of `Program.cs`

```csharp
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Information("Starting Up");
```

> Note that we hardcode the configuration of the Serilogger to reduce any potential failure to create it.

The wrap the whole startup process around a try catch block to log any potential exception to the Serilog sink.

Catch any exceptions:

```csharp
try {

    // ... Startup
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Application terminated unexpectedely");
}
finally
{
    Log.CloseAndFlush();
}
```