namespace Url.Shortener.Api.Domain.Url.Create;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCreateUrlServices(this IServiceCollection serviceCollection,
        Action<CodeGeneratorOptions> configureCodeGeneratorOptions) =>
        serviceCollection.AddUrlShortenerOptions(configureCodeGeneratorOptions)
                         .AddSingleton<ICodeGenerator, CodeGenerator>();

    private static IServiceCollection AddUrlShortenerOptions(this IServiceCollection serviceCollection,
        Action<CodeGeneratorOptions> configureCodeGeneratorOptions) =>
        serviceCollection.AddOptions<CodeGeneratorOptions>()
                         .Configure(configureCodeGeneratorOptions)
                         .ValidateDataAnnotations()
                         .ValidateOnStart()
                         .Services;
}