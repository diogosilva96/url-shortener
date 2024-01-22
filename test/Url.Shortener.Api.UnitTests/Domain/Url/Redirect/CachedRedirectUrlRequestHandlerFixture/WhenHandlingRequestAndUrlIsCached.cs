using AutoFixture;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Redirect;
using Url.Shortener.Api.UnitTests.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Redirect.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Redirect.CachedRedirectUrlRequestHandlerFixture;

public class WhenHandlingRequestAndUrlIsCached
{
    private readonly CachedRedirectUrlRequestHandler _cachedHandler;
    private readonly string _expectedResult;
    private readonly IRequestHandler<RedirectUrlRequest, string> _handler;
    private readonly IMemoryCache _memoryCache;
    private readonly RedirectUrlRequest _request;

    public WhenHandlingRequestAndUrlIsCached()
    {
        var fixture = new Fixture();

        _request = new(fixture.Create<string>());

        _expectedResult = fixture.Create<string>();
        _handler = Substitute.For<IRequestHandler<RedirectUrlRequest, string>>();

        _memoryCache = new MemoryCacheBuilder().Setup(CacheKeys.RedirectUrl(_request.ShortUrl), _expectedResult)
                                               .Build();

        _cachedHandler = new CachedRedirectUrlRequestHandlerBuilder().With(_handler)
                                                                .With(_memoryCache)
                                                                .Build();
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenNoRequestIsHandled()
    {
        await WhenHandlingAsync();

        await _handler.DidNotReceiveWithAnyArgs()
                      .Handle(Arg.Any<RedirectUrlRequest>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenAResultIsRetrieved()
    {
        var result = await WhenHandlingAsync();

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ThenTheExpectedResultIsRetrieved()
    {
        var result = await WhenHandlingAsync();

        Assert.Equal(_expectedResult, result);
    }

    private async Task<string> WhenHandlingAsync() => await _cachedHandler.Handle(_request);
}