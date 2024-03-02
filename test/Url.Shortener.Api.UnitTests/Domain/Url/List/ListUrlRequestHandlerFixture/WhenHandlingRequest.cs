using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.Domain.Url.List;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.List.Builder;
using Xunit;
using ListUrlRequest = Url.Shortener.Api.Domain.Url.List.ListUrlRequest;
using UrlMetadata = Url.Shortener.Data.Models.UrlMetadata;

namespace Url.Shortener.Api.UnitTests.Domain.Url.List.ListUrlRequestHandlerFixture;

public class WhenHandlingRequest
{
    private readonly int _expectedPageCount;
    private readonly UrlMetadata[] _expectedUrlMetadata;
    private readonly ListUrlRequestHandler _handler;
    private readonly ListUrlRequest _request;

    public WhenHandlingRequest()
    {
        _request = new(3, 1);

        var metadata = new List<UrlMetadata>
        {
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build()
        };

        _expectedUrlMetadata = metadata.OrderByDescending(x => x.CreatedAtUtc)
                                       .Skip((_request.Page - 1) * _request.PageSize)
                                       .Take(_request.PageSize)
                                       .ToArray();

        _expectedPageCount = (int)Math.Ceiling((double)metadata.Count / _request.PageSize);

        var dbContext = new UrlShortenerDbContextBuilder().With(metadata)
                                                          .Build();

        _handler = new ListUrlRequestHandlerBuilder().With(dbContext)
                                                     .Build();
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenTheExpectedPagedResultIsReturned()
    {
        var result = await WhenHandlingAsync();

        Assert.Multiple
        (
            () => Assert.Equal(_expectedUrlMetadata.Length, result.Data.Count()),
            () => Assert.Equal(_expectedPageCount, result.PageCount),
            () => Assert.Equal(_request.Page, result.Page),
            () => Assert.Equal(_request.PageSize, result.PageSize)
        );
    }

    [Fact]
    public async Task ThenTheExpectedUrlMetadataIsReturned()
    {
        var result = await WhenHandlingAsync();

        Assert.All(_expectedUrlMetadata,
            expectedMetadata => Assert.Single(result.Data, metadata => metadata.FullUrl == expectedMetadata.FullUrl &&
                                                                       metadata.Code == expectedMetadata.Code &&
                                                                       metadata.CreatedAtUtc == expectedMetadata.CreatedAtUtc));
    }

    private async Task<PagedResult<Contracts.UrlMetadata>> WhenHandlingAsync() => await _handler.Handle(_request, CancellationToken.None);
}