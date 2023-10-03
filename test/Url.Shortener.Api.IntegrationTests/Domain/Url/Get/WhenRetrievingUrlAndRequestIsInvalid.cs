using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.Get;

public sealed class WhenRetrievingUrlAndRequestIsInvalid : IntegrationTestBase
{
    private readonly string _shortUrl;

    public WhenRetrievingUrlAndRequestIsInvalid(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        var fixture = new Fixture();

        var metadata = new[]
        {
            new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                    .Build()
        };

        // long short url (invalid)
        _shortUrl = "EpyD2QL2ecThCbgX1flUmiHXtEpyD2QL2ecThCbgX1flUmiHXtA";

        webApplicationFactory.SeedData(context => context.UrlMetadata.AddRange(metadata));
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRetrievingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenABadRequestResponseIsReturned()
    {
        var response = await WhenRetrievingAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ThenValidationProblemsAreReturned()
    {
        var response = await WhenRetrievingAsync();

        var problem = (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        Assert.NotEmpty(problem.Errors);
    }

    private async Task<HttpResponseMessage> WhenRetrievingAsync() =>
        await Client.GetAsync(Urls.Api.Urls.Get(_shortUrl));
}