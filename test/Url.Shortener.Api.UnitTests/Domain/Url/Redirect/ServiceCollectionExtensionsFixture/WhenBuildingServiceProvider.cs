using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Redirect;
using Url.Shortener.Api.UnitTests.Builder;
using Url.Shortener.Data;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Redirect.ServiceCollectionExtensionsFixture;

public class WhenBuildingServiceProvider
{
    private readonly IServiceCollection _serviceCollection;

    public WhenBuildingServiceProvider() =>
        _serviceCollection = ServiceCollectionBuilder.Build()
                                                     .AddSingleton(Substitute.For<ApplicationDbContext>())
                                                     .AddLogging();

    [Fact]
    public void TheNoExceptionIsThrown()
    {
        var exception = Record.Exception(WhenBuilding);

        Assert.Null(exception);
    }

    [Fact]
    public void ThenAMemoryCacheCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotNull(provider.GetService<IMemoryCache>());
    }

    [Fact]
    public void ThenARedirectUrlRequestHandlerCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotNull(provider.GetService<IRequestHandler<RedirectUrlRequest, string>>());
    }

    [Fact]
    public void ThenACachedRedirectUrlRequestHandlerCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.IsType<CachedRedirectUrlRequestHandler>(provider.GetRequiredService<IRequestHandler<RedirectUrlRequest, string>>());
    }

    [Fact]
    public void ThenAKeyedRedirectUrlRequestHandlerCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotNull(provider.GetKeyedService<IRequestHandler<RedirectUrlRequest, string>>(ServiceKeys.RedirectUrlRequestHandler));
    }

    private IServiceProvider WhenBuilding() => _serviceCollection.AddRedirectUrlServices()
                                                                 .BuildServiceProvider();
}