using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.List;

public sealed class WhenListingUrlMetadataAndRequestIsInvalid : IntegrationTestBase
{
    private readonly int _page;
    private readonly int _pageSize;

    public WhenListingUrlMetadataAndRequestIsInvalid(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _page = 1;
        _pageSize = 10000; // invalid page size
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenListingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenABadRequestResponseIsReturned()
    {
        var response = await WhenListingAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ThenValidationProblemsAreReturned()
    {
        var response = await WhenListingAsync();

        var problem = (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        Assert.NotEmpty(problem.Errors);
    }

    private async Task<HttpResponseMessage> WhenListingAsync() =>
        await Client.GetAsync(Urls.Api.Urls.List(_pageSize, _page));
}