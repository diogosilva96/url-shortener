using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.Domain.Url;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.UrlEndpointsFixture;

public class WhenCreatingUrl
{
    private readonly string _expectedUrl;
    private readonly IMediator _mediator;
    private readonly CreateUrlRequest _request;

    public WhenCreatingUrl()
    {
        var fixture = new Fixture();

        _request = new()
        {
            Url = fixture.Create<string>()
        };

        _expectedUrl = fixture.Create<string>();

        _mediator = Substitute.For<IMediator>();
        _mediator.Send(Arg.Any<Api.Domain.Url.Create.CreateUrlRequest>(), Arg.Any<CancellationToken>())
                 .Returns(_expectedUrl);
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenCreatingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenACreateUrlRequestIsSent()
    {
        await WhenCreatingAsync();

        await _mediator.ReceivedWithAnyArgs(1)
                       .Send(Arg.Any<Api.Domain.Url.Create.CreateUrlRequest>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenTheExpectedCreateUrlRequestIsSent()
    {
        await WhenCreatingAsync();

        await _mediator.Received(1)
                       .Send(Arg.Is<Api.Domain.Url.Create.CreateUrlRequest>(x => x.Url == _request.Url), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenAnOkResultIsReturned()
    {
        var result = await WhenCreatingAsync();

        Assert.IsType<Ok<string>>(result);
    }

    [Fact]
    public async Task ThenAnOkResultIsReturnedWithTheExpectedUrl()
    {
        var result = await WhenCreatingAsync();

        var okResult = (result as Ok<string>)!;
        Assert.Equal(_expectedUrl, okResult.Value);
    }

    private async Task<IResult> WhenCreatingAsync() => await UrlEndpoints.CreateUrlAsync(_request, _mediator);
}