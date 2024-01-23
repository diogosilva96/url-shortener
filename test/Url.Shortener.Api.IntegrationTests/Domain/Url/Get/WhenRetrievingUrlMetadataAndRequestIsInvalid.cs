using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.Get;

public sealed class WhenRetrievingUrlMetadataAndRequestIsInvalid : IntegrationTestBase
{
    private readonly string _shortUrl;

    public WhenRetrievingUrlMetadataAndRequestIsInvalid(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        var fixture = new Fixture();
        var urlMetadata = new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                                  .Build();

        // short short url (invalid)
        _shortUrl = "abcd";

        webApplicationFactory.SeedData(context => context.Add(urlMetadata));
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRedirectingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenABadRequestResponseIsReturned()
    {
        var response = await WhenRedirectingAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ThenValidationProblemsAreReturned()
    {
        var response = await WhenRedirectingAsync();

        var problem = (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        Assert.NotEmpty(problem.Errors);
    }

    private async Task<HttpResponseMessage> WhenRedirectingAsync() =>
        await Client.GetAsync(Urls.Api.Urls.Get(_shortUrl));
}