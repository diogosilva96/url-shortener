using Url.Shortener.Api.Data;
using Url.Shortener.Api.Domain;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Exceptions;
using Url.Shortener.Api.Health;

namespace Url.Shortener.Api;

public static class DependencyInjectionConfigurator
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        IHostEnvironment hostEnvironment)
    {
        var dbConnectionString = configuration.GetConnectionString(ConnectionStringNames.ApplicationDatabase)!;

        return serviceCollection.AddEndpointsApiExplorer()
                                .AddSwaggerGen()
                                .AddDomainServices(ConfigureUrlShortenerOptions)
                                .AddExceptionServices()
                                .AddDataServices(dbConnectionString)
                                .AddHealthServices()
                                .AddAuthorization();

        void ConfigureUrlShortenerOptions(UrlShortenerOptions options) => configuration.GetSection(ConfigurationSectionNames.UrlShortenerOptions).Bind(options);
    }
}