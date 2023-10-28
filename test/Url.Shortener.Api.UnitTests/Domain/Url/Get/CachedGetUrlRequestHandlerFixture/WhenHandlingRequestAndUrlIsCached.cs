using AutoFixture;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.UnitTests.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Get.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Get.CachedGetUrlRequestHandlerFixture;

public class WhenHandlingRequestAndUrlIsCached
{
    private readonly CachedGetUrlRequestHandler _cachedHandler;
    private readonly string _expectedResult;
    private readonly IRequestHandler<GetUrlRequest, string> _handler;
    private readonly IMemoryCache _memoryCache;
    private readonly GetUrlRequest _request;

    public WhenHandlingRequestAndUrlIsCached()
    {
        var fixture = new Fixture();

        _request = new(fixture.Create<string>());

        _expectedResult = fixture.Create<string>();
        _handler = Substitute.For<IRequestHandler<GetUrlRequest, string>>();

        _memoryCache = new MemoryCacheBuilder().Setup(CacheKeys.GetUrl(_request.ShortUrl), _expectedResult)
                                               .Build();

        _cachedHandler = new CachedGetUrlRequestHandlerBuilder().With(_handler)
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
                      .Handle(Arg.Any<GetUrlRequest>(), Arg.Any<CancellationToken>());
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