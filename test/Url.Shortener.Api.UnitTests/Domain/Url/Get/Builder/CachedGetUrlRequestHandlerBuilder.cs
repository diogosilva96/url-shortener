using MediatR;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Get;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Get.Builder;

internal class CachedGetUrlRequestHandlerBuilder
{
    private IRequestHandler<GetUrlRequest, string> _handler;
    private IMemoryCache _memoryCache;

    public CachedGetUrlRequestHandlerBuilder()
    {
        _handler = Substitute.For<IRequestHandler<GetUrlRequest, string>>();
        _memoryCache = Substitute.For<IMemoryCache>();
    }

    public CachedGetUrlRequestHandler Build() => new(_handler, _memoryCache);

    public CachedGetUrlRequestHandlerBuilder With(IRequestHandler<GetUrlRequest, string> handler)
    {
        _handler = handler;

        return this;
    }

    public CachedGetUrlRequestHandlerBuilder With(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;

        return this;
    }
}