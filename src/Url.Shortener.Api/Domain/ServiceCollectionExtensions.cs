using Carter;
using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.Internal;
using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.Domain.Url.Create;

namespace Url.Shortener.Api.Domain;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection,
        Action<UrlShortenerOptions> configureUrlShortenerOptions)
    {
        return serviceCollection.AddCarter()
                                .AddMediatR(config =>
                                {
                                    config.Lifetime = ServiceLifetime.Transient;
                                    config.RegisterServicesFromAssemblyContaining<Program>();
                                    config.RequestPreProcessorsToRegister.Add(new(typeof(IRequestPreProcessor<>),
                                        typeof(ValidationProcessor<>), ServiceLifetime.Transient));
                                })
                                .AddSingleton<ISystemClock, SystemClock>() // TODO: use time provider (.NET 8)
                                .AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true)
                                .AddUrlServices(configureUrlShortenerOptions);
    }
}