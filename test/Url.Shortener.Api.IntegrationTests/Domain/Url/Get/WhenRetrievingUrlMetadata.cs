using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.Get;

public sealed class WhenRetrievingUrlMetadata : IntegrationTestBase
{
    private readonly string _code;

    public WhenRetrievingUrlMetadata(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        var fixture = new Fixture();
        var urlMetadata = new UrlMetadataBuilder().With(x => x.Code = fixture.Create<string>()[..15])
                                                  .Build();

        _code = urlMetadata.Code;

        webApplicationFactory.SeedData(context => context.Add(urlMetadata));
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRedirectingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenAnOkResponseIsReturned()
    {
        var response = await WhenRedirectingAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ThenTheExpectedUrlMetadataIsRetrieved()
    {
        var response = await WhenRedirectingAsync();

        var urlMetadata = await response.Content.ReadFromJsonAsync<UrlMetadata>();

        Assert.Multiple
        (
            () => Assert.NotNull(urlMetadata),
            () => Assert.Equal(_code, urlMetadata!.Code)
        );
    }

    private async Task<HttpResponseMessage> WhenRedirectingAsync() =>
        await Client.GetAsync(Urls.Api.Urls.Get(_code));
}