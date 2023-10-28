namespace Url.Shortener.Api.Domain.Url.Get;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGetUrlServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddMemoryCache()
                         .AddTransient<GetUrlRequestHandler>();
}