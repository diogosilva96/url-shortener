using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Utils;

public class IntegrationTestBase : IClassFixture<IntegrationTestWebApplicationFactory>
{
    protected IntegrationTestBase(IntegrationTestWebApplicationFactory webApplicationFactory)
    { }
}