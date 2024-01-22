using MediatR;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Redirect;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Redirect.Builder;

internal class CachedRedirectUrlRequestHandlerBuilder
{
    private IRequestHandler<RedirectUrlRequest, string> _handler;
    private IMemoryCache _memoryCache;

    public CachedRedirectUrlRequestHandlerBuilder()
    {
        _handler = Substitute.For<IRequestHandler<RedirectUrlRequest, string>>();
        _memoryCache = Substitute.For<IMemoryCache>();
    }

    public CachedRedirectUrlRequestHandler Build() => new(_handler, _memoryCache);

    public CachedRedirectUrlRequestHandlerBuilder With(IRequestHandler<RedirectUrlRequest, string> handler)
    {
        _handler = handler;

        return this;
    }

    public CachedRedirectUrlRequestHandlerBuilder With(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;

        return this;
    }
}