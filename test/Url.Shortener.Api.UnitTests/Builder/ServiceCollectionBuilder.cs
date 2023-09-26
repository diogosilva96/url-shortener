using Microsoft.Extensions.DependencyInjection;

namespace Url.Shortener.Api.UnitTests.Builder;

internal static class ServiceCollectionBuilder
{
    public static IServiceCollection Build() => new ServiceCollection();
}