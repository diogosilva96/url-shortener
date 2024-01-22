using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Url.Shortener.Api.Domain.Url.Redirect;

internal class CachedRedirectUrlRequestHandler : IRequestHandler<RedirectUrlRequest, string>
{
    private static readonly TimeSpan _relativeExpirationTimespan = TimeSpan.FromMinutes(5);
    private readonly IRequestHandler<RedirectUrlRequest, string> _handler;
    private readonly IMemoryCache _memoryCache;

    public CachedRedirectUrlRequestHandler([FromKeyedServices(ServiceKeys.RedirectUrlRequestHandler)] IRequestHandler<RedirectUrlRequest, string> handler,
        IMemoryCache memoryCache)
    {
        _handler = handler;
        _memoryCache = memoryCache;
    }

    public async Task<string> Handle(RedirectUrlRequest request, CancellationToken cancellationToken = default) =>
    (
        await _memoryCache.GetOrCreateAsync(CacheKeys.RedirectUrl(request.ShortUrl),
            async entry =>
            {
                var result = await _handler.Handle(request, cancellationToken);
                entry.AbsoluteExpirationRelativeToNow = _relativeExpirationTimespan;
                return result;
            })
    )!;
}