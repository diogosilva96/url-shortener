namespace Url.Shortener.Api.Domain.Url.Create;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCreateUrlServices(this IServiceCollection serviceCollection,
        Action<UrlShortenerOptions> configureUrlShortenerOptions) =>
        serviceCollection.AddUrlShortenerOptions(configureUrlShortenerOptions)
                         .AddSingleton<IUrlShortener, UrlShortener>();

    private static IServiceCollection AddUrlShortenerOptions(this IServiceCollection serviceCollection,
        Action<UrlShortenerOptions> configureUrlShortenerOptions) =>
        serviceCollection.AddOptions<UrlShortenerOptions>()
                         .Configure(configureUrlShortenerOptions)
                         .ValidateDataAnnotations()
                         .ValidateOnStart()
                         .Services;
}