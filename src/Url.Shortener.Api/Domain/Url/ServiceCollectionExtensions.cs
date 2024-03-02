using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Domain.Url.Redirect;

namespace Url.Shortener.Api.Domain.Url;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUrlServices(this IServiceCollection serviceCollection,
        Action<CodeGeneratorOptions> configureCodeGeneratorOptions) =>
        serviceCollection.AddCreateUrlServices(configureCodeGeneratorOptions)
                         .AddRedirectUrlServices();
}