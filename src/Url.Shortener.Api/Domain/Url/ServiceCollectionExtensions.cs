using Url.Shortener.Api.Domain.Url.Create;

namespace Url.Shortener.Api.Domain.Url;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUrlServices(this IServiceCollection serviceCollection, 
        Action<UrlShortenerOptions> configureUrlShortenerOptions) => serviceCollection.AddCreateUrlServices(configureUrlShortenerOptions);
}