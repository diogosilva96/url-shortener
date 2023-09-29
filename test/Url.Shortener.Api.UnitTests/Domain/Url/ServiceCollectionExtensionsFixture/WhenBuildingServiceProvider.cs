using Microsoft.Extensions.DependencyInjection;
using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Tests.Common.Domain.Url.Builder;
using Url.Shortener.Api.UnitTests.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.ServiceCollectionExtensionsFixture;

public class WhenBuildingServiceProvider
{
    private readonly Action<UrlShortenerOptions> _configureUrlShortenerOptions;
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
    public void TheNoExceptionIsThrown()
    {
        var exception = Record.Exception(WhenBuilding);

        Assert.Null(exception);
    }

    private IServiceProvider WhenBuilding() => _serviceCollection.AddUrlServices(_configureUrlShortenerOptions)
                                                                 .BuildServiceProvider();
}