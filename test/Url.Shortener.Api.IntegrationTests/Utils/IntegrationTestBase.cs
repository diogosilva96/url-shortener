using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Utils;

public class IntegrationTestBase : IClassFixture<IntegrationTestWebApplicationFactory>
{
    protected readonly HttpClient Client;

    protected IntegrationTestBase(IntegrationTestWebApplicationFactory webApplicationFactory) =>
        Client = webApplicationFactory.CreateClient(new()
        {
            AllowAutoRedirect = false
        });
}