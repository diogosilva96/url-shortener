using Url.Shortener.Data;

namespace Url.Shortener.Api.Health;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHealthServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddHealthChecks()
                         .AddDbContextCheck<UrlShortenerDbContext>("UrlShortenerDb", tags: new[] { HealthTags.Live })
                         .Services;
}