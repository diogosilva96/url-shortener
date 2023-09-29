using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Url.Shortener.Api.Health;
using Url.Shortener.Api.UnitTests.Builder;
using Url.Shortener.Data;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Health.ServiceCollectionExtensionsFixture;

public class WhenBuildingServiceProvider
{
    private readonly IServiceCollection _serviceCollection;

    public WhenBuildingServiceProvider() => _serviceCollection = ServiceCollectionBuilder.Build();

    [Fact]
    public void ThenNoExceptionIsThrown()
    {
        var exception = Record.Exception(WhenBuilding);

        Assert.Null(exception);
    }

    [Fact]
    public void ThenHealthCheckServiceOptionsCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotNull(provider.GetService<IOptions<HealthCheckServiceOptions>>());
    }

    [Fact]
    public void ThenAHealthCheckRegistrationCanBeRetrievedForTheUrlShortenerDbContext()
    {
        var provider = WhenBuilding();

        var options = provider.GetRequiredService<IOptions<HealthCheckServiceOptions>>();
        Assert.NotNull(options.Value.Registrations.Single(x => x.Name == nameof(UrlShortenerDbContext)));
    }

    private IServiceProvider WhenBuilding() => _serviceCollection.AddHealthServices()
                                                                 .BuildServiceProvider();
}