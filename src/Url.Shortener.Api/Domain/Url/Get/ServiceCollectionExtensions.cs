using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Url.Shortener.Api.Domain.Url.Get;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGetUrlServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddMemoryCache()
                         .RemoveAll(typeof(IRequestHandler<GetUrlRequest, string>))
                         .AddTransient<GetUrlRequestHandler>()
                         .AddTransient<IRequestHandler<GetUrlRequest, string>, CachedGetUrlRequestHandler>(s =>
                             new(s.GetRequiredService<GetUrlRequestHandler>(), s.GetRequiredService<IMemoryCache>()));
}