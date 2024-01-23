using Serilog;

namespace Url.Shortener.Api;

public static class LoggingConfigurator
{
    public static WebApplicationBuilder ConfigureSerilogAsOnlyLoggingProvider(this WebApplicationBuilder hostBuilder,
        IConfiguration configuration)
    {
        hostBuilder.Logging.ClearProviders();
        
        var logger = new LoggerConfiguration().MinimumLevel.Debug()
                                              .ReadFrom.Configuration(configuration)
                                              .CreateLogger();
        
        hostBuilder.Host.UseSerilog(logger);

        return hostBuilder;
    }
}