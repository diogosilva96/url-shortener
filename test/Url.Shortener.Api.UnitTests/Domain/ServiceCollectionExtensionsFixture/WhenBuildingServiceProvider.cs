using Microsoft.Extensions.DependencyInjection;
using Url.Shortener.Api.Domain;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Tests.Common.Domain.Url.Builder;
using Url.Shortener.Api.UnitTests.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.ServiceCollectionExtensionsFixture;

public class WhenBuildingServiceProvider
{
    private readonly Action<CodeGeneratorOptions> _configureUrlShortenerOptions;
    private readonly IServiceCollection _serviceCollection;

    public WhenBuildingServiceProvider()
    {
        var urlShortenerOptions = new UrlShortenerOptionsBuilder().Build();
        _configureUrlShortenerOptions = options =>
        {
            options.Characters = urlShortenerOptions.Characters;
            options.UrlSize = urlShortenerOptions.UrlSize;
        };

        _serviceCollection = ServiceCollectionBuilder.Build();
    }

    [Fact]
    public void ThenNoExceptionIsThrown()
    {
        var exception = Record.Exception(WhenBuilding);

        Assert.Null(exception);
    }

    [Fact]
    public void ThenATimeProviderCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotNull(provider.GetService<TimeProvider>());
    }

    private IServiceProvider WhenBuilding() => _serviceCollection.AddDomainServices(_configureUrlShortenerOptions)
                                                                 .BuildServiceProvider();
}