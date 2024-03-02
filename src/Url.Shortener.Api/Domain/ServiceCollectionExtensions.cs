using FluentValidation;
using MediatR.Pipeline;
using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.Domain.Url.Create;

namespace Url.Shortener.Api.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection,
        Action<CodeGeneratorOptions> configureCodeGeneratorOptions)
    {
        return serviceCollection.AddMediatR(config =>
                                {
                                    config.Lifetime = ServiceLifetime.Transient;
                                    config.RegisterServicesFromAssemblyContaining<Program>();
                                    config.RequestPreProcessorsToRegister.Add(new(typeof(IRequestPreProcessor<>),
                                        typeof(ValidationProcessor<>), ServiceLifetime.Transient));
                                })
                                .AddSingleton<TimeProvider>(_ => TimeProvider.System)
                                .AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true)
                                .AddUrlServices(configureCodeGeneratorOptions);
    }
}