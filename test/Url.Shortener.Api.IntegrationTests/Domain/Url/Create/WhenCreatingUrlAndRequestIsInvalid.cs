using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.Create;

public sealed class WhenCreatingUrlAndRequestIsInvalid : IntegrationTestBase
{
    private readonly CreateUrlRequest _request;

    public WhenCreatingUrlAndRequestIsInvalid(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        var fixture = new Fixture();

        _request = new()
        {
            Url = fixture.Create<string>() // invalid url (non-https)
        };

        var metadata = new[]
        {
            new UrlMetadataBuilder().With(x => x.Code = fixture.Create<string>()[..15])
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.Code = fixture.Create<string>()[..15])
                                    .Build()
        };

        webApplicationFactory.SeedData(context => context.UrlMetadata.AddRange(metadata));
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenCreatingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenABadRequestResponseIsReturned()
    {
        var response = await WhenCreatingAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ThenValidationProblemsAreReturned()
    {
        var response = await WhenCreatingAsync();

        var problem = (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        Assert.NotEmpty(problem.Errors);
    }

    private async Task<HttpResponseMessage> WhenCreatingAsync() =>
        await Client.PostAsync(Urls.Api.Urls.Create, JsonContent.Create(_request));
}