using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Url.Shortener.Api.Domain.Url.Redirect;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedirectUrlServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddMemoryCache()
                         .RemoveAll(typeof(IRequestHandler<RedirectUrlRequest, string>))
                         .AddKeyedTransient<IRequestHandler<RedirectUrlRequest, string>, RedirectUrlRequestHandler>(ServiceKeys.RedirectUrlRequestHandler)
                         .AddTransient<IRequestHandler<RedirectUrlRequest, string>, CachedRedirectUrlRequestHandler>();
}