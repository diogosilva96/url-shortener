using Serilog;

namespace Url.Shortener.Api;

public static class LoggingConfigurator
{
    public static IHostBuilder ConfigureSerilogAsOnlyLoggingProvider(this IHostBuilder hostBuilder,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);
        ArgumentNullException.ThrowIfNull(configuration);

        var logger = new LoggerConfiguration().MinimumLevel.Debug()
                                              .ReadFrom.Configuration(configuration)
                                              .CreateLogger();

        return hostBuilder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders())
                          .UseSerilog(logger);
    }
}