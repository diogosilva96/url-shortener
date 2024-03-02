using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;
using UrlMetadata = Url.Shortener.Data.Models.UrlMetadata;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.List;

public sealed class WhenListingUrlMetadata : IntegrationTestBase
{
    private readonly int _expectedPageCount;
    private readonly string[] _expectedUrlMetadataCodes;
    private readonly int _page;
    private readonly int _pageSize;

    public WhenListingUrlMetadata(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _page = 2;
        _pageSize = 2;
        var fixture = new Fixture();
        var metadata = new List<UrlMetadata>
        {
            new UrlMetadataBuilder().With(x => x.Code = fixture.Create<string>()[..15])
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.Code = fixture.Create<string>()[..15])
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.Code = fixture.Create<string>()[..15])
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.Code = fixture.Create<string>()[..15])
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.Code = fixture.Create<string>()[..15])
                                    .Build()
        };

        webApplicationFactory.SeedData(context => context.AddRange(metadata));

        _expectedUrlMetadataCodes = metadata.OrderByDescending(x => x.CreatedAtUtc)
                                            .Skip((_page - 1) * _pageSize)
                                            .Take(_pageSize)
                                            .Select(x => x.Code)
                                            .ToArray();

        _expectedPageCount = (int)Math.Ceiling((double)metadata.Count / _pageSize);
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenListingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenAnOkResponseIsReturned()
    {
        var response = await WhenListingAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ThenTheExpectedPagedResultIsRetrieved()
    {
        var response = await WhenListingAsync();

        var result = (await response.Content.ReadFromJsonAsync<PagedResult<Contracts.UrlMetadata>>())!;

        Assert.Multiple
        (
            () => Assert.Equal(_expectedUrlMetadataCodes.Length, result.Data.Count()),
            () => Assert.All(_expectedUrlMetadataCodes, expectedCode => Assert.Single(result.Data, metadata => metadata.Code == expectedCode)),
            () => Assert.Equal(_expectedPageCount, result.PageCount),
            () => Assert.Equal(_page, result.Page),
            () => Assert.Equal(_pageSize, result.PageSize)
        );
    }


    private async Task<HttpResponseMessage> WhenListingAsync() =>
        await Client.GetAsync(Urls.Api.Urls.List(_pageSize, _page));
}