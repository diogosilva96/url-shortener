using System.Net;
using AutoFixture;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.Get;

public sealed class WhenRetrievingUrl : IntegrationTestBase
{
    private readonly string _expectedRedirectUrl;
    private readonly string _shortUrl;

    public WhenRetrievingUrl(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        var fixture = new Fixture();
        var urlMetadata = new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                                  .Build();

        _shortUrl = urlMetadata.ShortUrl;
        _expectedRedirectUrl = urlMetadata.FullUrl;

        webApplicationFactory.SeedData(context => context.Add(urlMetadata));
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRetrievingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenAMovedPermanentlyResponseIsReturned()
    {
        var response = await WhenRetrievingAsync();

        Assert.Equal(HttpStatusCode.PermanentRedirect, response.StatusCode);
    }

    [Fact]
    public async Task ThenTheExpectedRedirectUrlIsContainedInTheLocationHeader()
    {
        var response = await WhenRetrievingAsync();

        Assert.Equal(_expectedRedirectUrl, response.Headers.Location!.ToString());
    }

    private async Task<HttpResponseMessage> WhenRetrievingAsync() =>
        await Client.GetAsync(Urls.Api.Urls.Get(_shortUrl));
}