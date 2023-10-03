using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.Domain.Url.Get;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.UrlEndpointsFixture;

public class WhenRetrievingUrl
{
    private readonly string _expectedUrl;
    private readonly IMediator _mediator;
    private readonly string _shortUrl;

    public WhenRetrievingUrl()
    {
        var fixture = new Fixture();

        _shortUrl = fixture.Create<string>();
        _expectedUrl = fixture.Create<string>();

        _mediator = Substitute.For<IMediator>();
        _mediator.Send(Arg.Any<GetUrlRequest>(), Arg.Any<CancellationToken>())
                 .Returns(_expectedUrl);
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRetrievingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenAGetUrlRequestIsSent()
    {
        await WhenRetrievingAsync();

        await _mediator.ReceivedWithAnyArgs(1)
                       .Send(Arg.Any<GetUrlRequest>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenTheExpectedGetUrlRequestIsSent()
    {
        await WhenRetrievingAsync();

        await _mediator.Received(1)
                       .Send(Arg.Is<GetUrlRequest>(x => x.ShortUrl == _shortUrl), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenARedirectResultIsReturned()
    {
        var result = await WhenRetrievingAsync();

        Assert.IsType<RedirectHttpResult>(result);
    }

    [Fact]
    public async Task ThenARedirectResultIsReturnedWithTheExpectedShortUrl()
    {
        var result = await WhenRetrievingAsync();

        var redirectResult = (result as RedirectHttpResult)!;
        Assert.Equal(_expectedUrl, redirectResult.Url);
    }

    private async Task<IResult> WhenRetrievingAsync() => await UrlEndpoints.GetUrlAsync(_shortUrl, _mediator);
}