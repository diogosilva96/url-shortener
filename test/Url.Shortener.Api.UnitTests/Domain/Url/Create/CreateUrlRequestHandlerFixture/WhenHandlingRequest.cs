﻿using AutoFixture;
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
    private readonly ApplicationDbContext _dbContext;
    private readonly DateTimeOffset _expectedDateTime;
    private readonly string _expectedShortUrl;
    private readonly CreateUrlRequestHandler _handler;
    private readonly CreateUrlRequest _request;
    private readonly TimeProvider _timeProvider;
    private readonly IUrlShortener _urlShortener;

    public WhenHandlingRequest()
    {
        var fixture = new Fixture();

        _request = new(fixture.Create<string>());

        _dbContext = new UrlShortenerDbContextBuilder().Build();

        _expectedShortUrl = fixture.Create<string>();
        _urlShortener = Substitute.For<IUrlShortener>();
        _urlShortener.GenerateUrl().Returns(_expectedShortUrl);

        _expectedDateTime = fixture.Create<DateTimeOffset>();
        _timeProvider = Substitute.For<TimeProvider>();
        _timeProvider.GetUtcNow().Returns(_expectedDateTime);

        _handler = new CreateUrlRequestHandlerBuilder().With(_dbContext)
                                                       .With(_urlShortener)
                                                       .With(_timeProvider)
                                                       .Build();
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenCurrentTimeIsRetrieved()
    {
        await WhenHandlingAsync();

        _timeProvider.ReceivedWithAnyArgs(1)
                     .GetUtcNow();
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
                  .Add(Arg.Is<UrlMetadata>(x => x.ShortUrl == _expectedShortUrl &&
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
    public async Task ThenTheExpectedShortUrlIsReturned()
    {
        var url = await WhenHandlingAsync();

        Assert.Equal(_expectedShortUrl, url);
    }

    private async Task<string> WhenHandlingAsync() => await _handler.Handle(_request);
}