using AutoFixture;
using MediatR;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Redirect;
using Url.Shortener.Api.UnitTests.Domain.Url.Redirect.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Redirect.CachedRedirectUrlRequestHandlerFixture;

public class WhenHandlingRequest
{
    private readonly CachedRedirectUrlRequestHandler _cachedHandler;
    private readonly string _expectedResult;
    private readonly IRequestHandler<RedirectUrlRequest, string> _handler;
    private readonly RedirectUrlRequest _request;

    public WhenHandlingRequest()
    {
        var fixture = new Fixture();

        _request = new(fixture.Create<string>());

        _expectedResult = fixture.Create<string>();
        _handler = Substitute.For<IRequestHandler<RedirectUrlRequest, string>>();
        _handler.Handle(Arg.Any<RedirectUrlRequest>(), Arg.Any<CancellationToken>())
                .Returns(_expectedResult);

        _cachedHandler = new CachedRedirectUrlRequestHandlerBuilder().With(_handler)
                                                                     .Build();
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenARequestIsHandled()
    {
        await WhenHandlingAsync();

        await _handler.ReceivedWithAnyArgs(1)
                      .Handle(Arg.Any<RedirectUrlRequest>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenTheExpectedRequestIsHandled()
    {
        await WhenHandlingAsync();

        await _handler.Received(1)
                      .Handle(_request, Arg.Any<CancellationToken>());
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