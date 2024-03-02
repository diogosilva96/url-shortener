using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.Domain.Url.Get;
using Xunit;
using UrlMetadataBuilder = Url.Shortener.Api.UnitTests.Contracts.Builder.UrlMetadataBuilder;

namespace Url.Shortener.Api.UnitTests.Domain.Url.UrlEndpointsFixture;

public class WhenRetrievingUrl
{
    private readonly string _code;
    private readonly UrlMetadata _expectedUrlMetadata;
    private readonly IMediator _mediator;

    public WhenRetrievingUrl()
    {
        var fixture = new Fixture();

        _code = fixture.Create<string>();

        _expectedUrlMetadata = new UrlMetadataBuilder().Build();

        _mediator = Substitute.For<IMediator>();
        _mediator.Send(Arg.Any<GetUrlRequest>(), Arg.Any<CancellationToken>())
                 .Returns(_expectedUrlMetadata);
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
                       .Send(Arg.Is<GetUrlRequest>(x => x.Code == _code), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenAnOkResultIsReturned()
    {
        var result = await WhenRetrievingAsync();

        Assert.IsType<Ok<UrlMetadata>>(result);
    }

    [Fact]
    public async Task ThenAnOkResultIsReturnedWithTheExpectedUrl()
    {
        var result = await WhenRetrievingAsync();

        var okResult = (result as Ok<UrlMetadata>)!;
        Assert.Equal(_expectedUrlMetadata, okResult.Value);
    }

    private async Task<IResult> WhenRetrievingAsync() => await UrlEndpoints.GetUrlAsync(_code, _mediator);
}