﻿using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.UnitTests.Builder;
using Url.Shortener.Data;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Get.ServiceCollectionExtensionsFixture;

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
    public void ThenAGetUrlRequestHandlerCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotNull(provider.GetService<IRequestHandler<GetUrlRequest, string>>());
    }

    [Fact]
    public void ThenACachedGetUrlRequestHandlerCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.IsType<CachedGetUrlRequestHandler>(provider.GetRequiredService<IRequestHandler<GetUrlRequest, string>>());
    }

    [Fact]
    public void ThenAKeyedGetUrlRequestHandlerCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotNull(provider.GetKeyedService<IRequestHandler<GetUrlRequest, string>>(ServiceKeys.GetUrlRequestHandler));
    }

    private IServiceProvider WhenBuilding() => _serviceCollection.AddGetUrlServices()
                                                                 .BuildServiceProvider();
}