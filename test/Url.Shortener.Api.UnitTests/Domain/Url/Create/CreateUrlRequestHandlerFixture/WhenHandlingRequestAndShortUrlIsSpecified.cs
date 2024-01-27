using AutoFixture;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Url.Shortener.Data;
using Url.Shortener.Data.Models;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestHandlerFixture;

public class WhenHandlingRequestAndCodeIsSpecified
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DateTimeOffset _expectedDateTime;
    private readonly CreateUrlRequestHandler _handler;
    private readonly CreateUrlRequest _request;
    private readonly TimeProvider _timeProvider;
    private readonly ICodeGenerator _codeGenerator;

    public WhenHandlingRequestAndCodeIsSpecified()
    {
        var fixture = new Fixture();

        _request = new(fixture.Create<string>(), fixture.Create<string>());

        _dbContext = new UrlShortenerDbContextBuilder().Build();

        _codeGenerator = Substitute.For<ICodeGenerator>();

        _expectedDateTime = fixture.Create<DateTimeOffset>();
        _timeProvider = Substitute.For<TimeProvider>();
        _timeProvider.GetUtcNow().Returns(_expectedDateTime);

        _handler = new CreateUrlRequestHandlerBuilder().With(_dbContext)
                                                       .With(_codeGenerator)
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
    public async Task ThenNoUrlIsGenerated()
    {
        await WhenHandlingAsync();

        _codeGenerator.DidNotReceiveWithAnyArgs()
                     .GenerateCode();
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
                  .Add(Arg.Is<UrlMetadata>(x => x.Code == _request.Code &&
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
    public async Task ThenTheExpectedCodeIsReturned()
    {
        var url = await WhenHandlingAsync();

        Assert.Equal(_request.Code, url);
    }

    private async Task<string> WhenHandlingAsync() => await _handler.Handle(_request);
}