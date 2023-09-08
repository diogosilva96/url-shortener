using Carter;
using FluentValidation;

namespace Url.Shortener.Api.Domain.CreateUrl;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCreateUrlServices(this IServiceCollection serviceCollection, Action<UrlShortenerOptions> configureUrlShortenerOptions)
    {
        return serviceCollection.AddUrlShortenerOptions(configureUrlShortenerOptions)
                                .AddSingleton<IUrlShortener, UrlShortener>();
    }
    
    private static IServiceCollection AddUrlShortenerOptions(this IServiceCollection serviceCollection, Action<UrlShortenerOptions> configureUrlShortenerOptions)
    {
        return serviceCollection.AddOptions<UrlShortenerOptions>()
                                .Configure(configureUrlShortenerOptions)
                                .ValidateDataAnnotations()
                                .ValidateOnStart()
                                .Services;
    }
}