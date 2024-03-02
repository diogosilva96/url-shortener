using Microsoft.Extensions.DependencyInjection;

namespace Url.Shortener.Api.UnitTests.Builder;

public static class ServiceCollectionBuilder
{
    public static IServiceCollection Build() => new ServiceCollection();
}