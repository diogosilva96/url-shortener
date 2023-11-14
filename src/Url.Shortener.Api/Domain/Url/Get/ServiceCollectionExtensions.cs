using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Url.Shortener.Api.Domain.Url.Get;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGetUrlServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddMemoryCache()
                         .RemoveAll(typeof(IRequestHandler<GetUrlRequest, string>))
                         .AddKeyedTransient<IRequestHandler<GetUrlRequest, string>, GetUrlRequestHandler>(ServiceKeys.GetUrlRequestHandler)
                         .AddTransient<IRequestHandler<GetUrlRequest, string>, CachedGetUrlRequestHandler>();
}