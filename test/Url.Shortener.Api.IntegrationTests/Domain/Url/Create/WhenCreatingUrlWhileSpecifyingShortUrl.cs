using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.Create;

public sealed class WhenCreatingUrlWhileSpecifyingShortUrl : IntegrationTestBase
{
    private readonly CreateUrlRequest _request;

    public WhenCreatingUrlWhileSpecifyingShortUrl(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        var fixture = new Fixture();

        _request = new()
        {
            Url = $"https://{fixture.Create<string>()}.com/",
            ShortUrl = "my-short-url"
        };

        var metadata = new[]
        {
            new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                    .Build()
        };

        webApplicationFactory.SeedData(context => context.UrlMetadata.AddRange(metadata));
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRetrievingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenAnOkResponseIsReturned()
    {
        var response = await WhenRetrievingAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ThenTheExpectedShortUrlIsReturned()
    {
        var response = await WhenRetrievingAsync();

        var shortUrl = await response.Content.ReadAsStringAsync();

        Assert.Contains(_request.ShortUrl!, shortUrl);
    }

    private async Task<HttpResponseMessage> WhenRetrievingAsync() =>
        await Client.PostAsync(Urls.Api.Urls.Create, JsonContent.Create(_request));
}