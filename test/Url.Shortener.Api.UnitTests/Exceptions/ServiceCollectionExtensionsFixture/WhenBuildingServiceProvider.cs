using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Url.Shortener.Api.Exceptions;
using Url.Shortener.Api.UnitTests.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Exceptions.ServiceCollectionExtensionsFixture;

public class WhenBuildingServiceProvider
{
    private readonly IServiceCollection _serviceCollection;

    public WhenBuildingServiceProvider() => _serviceCollection = ServiceCollectionBuilder.Build()
                                                                                         .AddLogging();

    [Fact]
    public void ThenNoExceptionIsThrown()
    {
        var exception = Record.Exception(WhenBuilding);

        Assert.Null(exception);
    }

    [Fact]
    public void ThenExceptionHandlersCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotEmpty(provider.GetServices<IExceptionHandler>());
    }

    [Fact]
    public void ThenADomainExceptionHandlerCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotNull(provider.GetServices<IExceptionHandler>()
                               .FirstOrDefault(x => x is DomainExceptionHandler));
    }

    private IServiceProvider WhenBuilding() => _serviceCollection.AddExceptionServices()
                                                                 .BuildServiceProvider();
}