using Carter;
using Url.Shortener.Api.Data;

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
                                .AddCarter()
                                .AddSwaggerGen()
                                .AddDataServices(dbConnectionString);
    }
}