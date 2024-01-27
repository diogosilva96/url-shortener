using AutoFixture;
using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Get.Builder;
using Url.Shortener.Data.Models;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Get.GetUrlRequestHandlerFixture;

public class WhenHandlingRequest
{
    private readonly UrlMetadata _expectedUrlMetadata;
    private readonly GetUrlRequestHandler _handler;
    private readonly GetUrlRequest _request;

    public WhenHandlingRequest()
    {
        var fixture = new Fixture();

        _request = new(fixture.Create<string>());

        _expectedUrlMetadata = new UrlMetadataBuilder().With(x => x.Code = _request.Code)
                                                       .Build();

        var metadata = new List<UrlMetadata>()
        {
            _expectedUrlMetadata,
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build()
        };

        var dbContext = new UrlShortenerDbContextBuilder().With(metadata)
                                                           .Build();

        _handler = new GetUrlRequestHandlerBuilder().With(dbContext)
                                                    .Build();
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenUrlMetadataIsReturned()
    {
        var metadata = await WhenHandlingAsync();

        Assert.NotNull(metadata);
    }

    [Fact]
    public async Task ThenTheExpectedUrlMetadataIsReturned()
    {
        var metadata = await WhenHandlingAsync();

        Assert.True(_expectedUrlMetadata.Code == metadata.Code && 
                    _expectedUrlMetadata.FullUrl == metadata.FullUrl &&
                    _expectedUrlMetadata.CreatedAtUtc == metadata.CreatedAtUtc);
    }

    private async Task<Contracts.UrlMetadata> WhenHandlingAsync() => await _handler.Handle(_request, CancellationToken.None);
}