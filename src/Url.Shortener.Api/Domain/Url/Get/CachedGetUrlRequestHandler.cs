using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Url.Shortener.Api.Domain.Url.Get;

internal class CachedGetUrlRequestHandler : IRequestHandler<GetUrlRequest, string>
{
    private static readonly TimeSpan _relativeExpirationTimespan = TimeSpan.FromMinutes(5);
    private readonly IRequestHandler<GetUrlRequest, string> _handler;
    private readonly IMemoryCache _memoryCache;

    public CachedGetUrlRequestHandler(IRequestHandler<GetUrlRequest, string> handler, IMemoryCache memoryCache)
    {
        _handler = handler;
        _memoryCache = memoryCache;
    }

    public async Task<string> Handle(GetUrlRequest request, CancellationToken cancellationToken = default) =>
    (
        await _memoryCache.GetOrCreateAsync(CacheKeys.GetUrl(request.ShortUrl),
            async entry =>
            {
                var result = await _handler.Handle(request, cancellationToken);
                entry.AbsoluteExpirationRelativeToNow = _relativeExpirationTimespan;
                return result;
            })
    )!;
}