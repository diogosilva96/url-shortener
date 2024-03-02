using Microsoft.Extensions.DependencyInjection;
using Url.Shortener.Api.Endpoints;
using Url.Shortener.Api.UnitTests.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Endpoints.ServiceCollectionExtensionsFixture;

public class WhenBuildingServiceProvider
{
    private readonly IServiceCollection _serviceCollection;

    public WhenBuildingServiceProvider()
    {

        _serviceCollection = ServiceCollectionBuilder.Build();
    }

    [Fact]
    public void ThenNoExceptionIsThrown()
    {
        var exception = Record.Exception(WhenBuilding);

        Assert.Null(exception);
    }

    [Fact]
    public void ThenEndpointsCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotEmpty(provider.GetServices<IEndpoint>());
    }

    private IServiceProvider WhenBuilding() => _serviceCollection.AddEndpoints(typeof(Program).Assembly)
                                                                 .BuildServiceProvider();
}