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

        return serviceCollection.AddControllers().Services
                                .AddEndpointsApiExplorer()
                                .AddSwaggerGen();
    }
}