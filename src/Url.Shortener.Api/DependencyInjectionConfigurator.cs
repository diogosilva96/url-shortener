using Carter;
using Url.Shortener.Api.Data;
using Url.Shortener.Api.Domain;

namespace Url.Shortener.Api;

public static class DependencyInjectionConfigurator
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(hostEnvironment);

        var dbConnectionString = configuration.GetConnectionString(ConnectionStringNames.MainDatabase)!;

        return serviceCollection.AddControllers().Services
                                .AddEndpointsApiExplorer()
                                .AddSwaggerGen()
                                .AddDomainServices()
                                .AddDataServices(dbConnectionString);
    }
}