using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.Domain.Url.Redirect;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.UrlEndpointsFixture;

public class WhenRetrievingUrl
{
    private readonly string _expectedUrl;
    private readonly IMediator _mediator;
    private readonly string _code;

    public WhenRetrievingUrl()
    {
        var fixture = new Fixture();

        _code = fixture.Create<string>();
        _expectedUrl = fixture.Create<string>();

        _mediator = Substitute.For<IMediator>();
        _mediator.Send(Arg.Any<RedirectUrlRequest>(), Arg.Any<CancellationToken>())
                 .Returns(_expectedUrl);
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRedirectingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenARedirectUrlRequestIsSent()
    {
        await WhenRedirectingAsync();

        await _mediator.ReceivedWithAnyArgs(1)
                       .Send(Arg.Any<RedirectUrlRequest>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenTheExpectedRedirectUrlRequestIsSent()
    {
        await WhenRedirectingAsync();

        await _mediator.Received(1)
                       .Send(Arg.Is<RedirectUrlRequest>(x => x.Code == _code), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenARedirectResultIsReturned()
    {
        var result = await WhenRedirectingAsync();

        Assert.IsType<RedirectHttpResult>(result);
    }

    [Fact]
    public async Task ThenARedirectResultIsReturnedWithTheExpectedCode()
    {
        var result = await WhenRedirectingAsync();

        var redirectResult = (result as RedirectHttpResult)!;
        Assert.Equal(_expectedUrl, redirectResult.Url);
    }

    private async Task<IResult> WhenRedirectingAsync() => await UrlEndpoints.RedirectUrlAsync(_code, _mediator);
}