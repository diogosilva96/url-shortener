using AutoFixture;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Url.Shortener.Data;
using Url.Shortener.Data.Models;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestHandlerFixture;

public class WhenHandlingRequestAndGeneratedCodeIsAlreadyInUse
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DateTimeOffset _expectedDateTime;
    private readonly int _expectedGeneratedUrlCount;
    private readonly string _expectedUrl;
    private readonly CreateUrlRequestHandler _handler;
    private readonly CreateUrlRequest _request;
    private readonly TimeProvider _timeProvider;
    private readonly ICodeGenerator _codeGenerator;

    public WhenHandlingRequestAndGeneratedCodeIsAlreadyInUse()
    {
        var fixture = new Fixture();

        _request = new(fixture.Create<string>());

        var urlMetadata = new[]
        {
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build()
        };

        _dbContext = new UrlShortenerDbContextBuilder().With(urlMetadata)
                                                       .Build();
        _expectedUrl = fixture.Create<string>();

        var generatedUrls = new[]
        {
            urlMetadata[0].Code,
            urlMetadata[1].Code,
            _expectedUrl
        };
        _codeGenerator = Substitute.For<ICodeGenerator>();
        _codeGenerator.GenerateCode().Returns(generatedUrls[0], generatedUrls[1..]);

        _expectedGeneratedUrlCount = generatedUrls.Length;

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
    public async Task ThenAUrlIsGenerated()
    {
        await WhenHandlingAsync();

        _codeGenerator.ReceivedWithAnyArgs(_expectedGeneratedUrlCount)
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
                  .Add(Arg.Is<UrlMetadata>(x => x.Code == _expectedUrl &&
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