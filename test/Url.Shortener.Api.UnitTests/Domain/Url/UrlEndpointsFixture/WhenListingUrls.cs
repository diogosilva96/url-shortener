using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.Domain.Url.List;
using Url.Shortener.Api.UnitTests.Contracts.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.UrlEndpointsFixture;

public class WhenListingUrls
{
    private readonly PagedResult<UrlMetadata> _expectedResult;
    private readonly IMediator _mediator;
    private readonly int _page;
    private readonly int _pageSize;

    public WhenListingUrls()
    {
        var fixture = new Fixture();

        _page = fixture.Create<int>();
        _pageSize = fixture.Create<int>();

        _expectedResult = new PagedResultBuilder<UrlMetadata>().Build();
        _mediator = Substitute.For<IMediator>();
        _mediator.Send(Arg.Any<ListUrlRequest>(), Arg.Any<CancellationToken>())
                 .Returns(_expectedResult);
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenListingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenAGetUrlRequestIsSent()
    {
        await WhenListingAsync();

        await _mediator.ReceivedWithAnyArgs(1)
                       .Send(Arg.Any<ListUrlRequest>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenTheExpectedGetUrlRequestIsSent()
    {
        await WhenListingAsync();

        await _mediator.Received(1)
                       .Send(Arg.Is<ListUrlRequest>(x => x.PageSize == _pageSize && x.Page == _page),
                           Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenAnOkResultIsReturned()
    {
        var result = await WhenListingAsync();

        Assert.IsType<Ok<PagedResult<UrlMetadata>>>(result);
    }

    [Fact]
    public async Task ThenAnOkResultIsReturnedWithTheExpectedUrl()
    {
        var result = await WhenListingAsync();

        var okResult = (result as Ok<PagedResult<UrlMetadata>>)!;
        Assert.Equal(_expectedResult, okResult.Value);
    }

    private async Task<IResult> WhenListingAsync() => await UrlEndpoints.ListUrlsAsync(_mediator, pageSize: _pageSize, page: _page);
}