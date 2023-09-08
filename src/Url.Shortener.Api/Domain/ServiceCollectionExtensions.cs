using Carter;

namespace Url.Shortener.Api.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddCarter();
    }
}