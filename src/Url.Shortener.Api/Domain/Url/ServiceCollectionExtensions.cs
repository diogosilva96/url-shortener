using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Domain.Url.Get;

namespace Url.Shortener.Api.Domain.Url;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUrlServices(this IServiceCollection serviceCollection,
        Action<UrlShortenerOptions> configureUrlShortenerOptions) =>
        serviceCollection.AddCreateUrlServices(configureUrlShortenerOptions)
                         .AddGetUrlServices();
}