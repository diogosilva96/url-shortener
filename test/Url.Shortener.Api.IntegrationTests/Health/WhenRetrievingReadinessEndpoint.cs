using System.Net;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Health;

public class WhenRetrievingReadinessEndpoint : IntegrationTestBase
{
    private readonly HttpClient _client;

    public WhenRetrievingReadinessEndpoint(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory) =>
        _client = webApplicationFactory.CreateClient();

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

    private async Task<HttpResponseMessage> WhenRetrievingAsync() => await _client.GetAsync("health/ready");
}