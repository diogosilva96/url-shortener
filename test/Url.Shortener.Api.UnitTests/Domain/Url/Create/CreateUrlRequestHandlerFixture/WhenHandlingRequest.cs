using AutoFixture;
using Microsoft.Extensions.Internal;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Url.Shortener.Data;
using Url.Shortener.Data.Models;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestHandlerFixture;

public class WhenHandlingRequest
{
    private readonly UrlShortenerDbContext _dbContext;
    private readonly DateTimeOffset _expectedDateTime;
    private readonly string _expectedUrl;
    private readonly CreateUrlRequestHandler _handler;
    private readonly CreateUrlRequest _request;
    private readonly IUrlShortener _urlShortener;

    public WhenHandlingRequest()
    {
        var fixture = new Fixture();

        _request = new(fixture.Create<string>());

        _dbContext = new UrlShortenerDbContextBuilder().Build();

        _expectedUrl = fixture.Create<string>();
        _urlShortener = Substitute.For<IUrlShortener>();
        _urlShortener.GenerateUrl().Returns(_expectedUrl);

        _expectedDateTime = fixture.Create<DateTimeOffset>();
        var clock = Substitute.For<ISystemClock>();
        clock.UtcNow.Returns(_expectedDateTime);

        _handler = new CreateUrlRequestHandlerBuilder().With(_dbContext)
                                                       .With(_urlShortener)
                                                       .With(clock)
                                                       .Build();
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenAUrlIsGenerated()
    {
        await WhenHandlingAsync();

        _urlShortener.ReceivedWithAnyArgs(1)
                     .GenerateUrl();
    }

    [Fact]
    public async Task ThenUrlMetadataIsAdded()
    {
        await WhenHandlingAsync();

        _dbContext.UrlMetadata
                  .ReceivedWithAnyArgs(1)
                  .Add(Arg.Any<UrlMetadata>());
    }

    [Fact]
    public async Task ThenTheExpectedUrlMetadataIsAdded()
    {
        await WhenHandlingAsync();

        _dbContext.UrlMetadata
                  .Received(1)
                  .Add(Arg.Is<UrlMetadata>(x => x.ShortUrl == _expectedUrl &&
                                                x.FullUrl == _request.Url &&
                                                x.CreatedAtUtc == _expectedDateTime));
    }

    [Fact]
    public async Task ThenChangesAreSaved()
    {
        await WhenHandlingAsync();

        await _dbContext.ReceivedWithAnyArgs(1)
                        .SaveChangesAsync();
    }

    [Fact]
    public async Task ThenAUrlIsReturned()
    {
        var url = await WhenHandlingAsync();

        Assert.NotEmpty(url);
    }

    [Fact]
    public async Task ThenTheExpectedUrlIsReturned()
    {
        var url = await WhenHandlingAsync();

        Assert.Equal(_expectedUrl, url);
    }

    private async Task<string> WhenHandlingAsync() => await _handler.Handle(_request);
}