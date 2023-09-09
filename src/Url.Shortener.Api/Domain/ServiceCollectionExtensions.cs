using Carter;
using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.Internal;
using Url.Shortener.Api.Domain.CreateUrl;

namespace Url.Shortener.Api.Domain;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection, Action<UrlShortenerOptions> configureUrlShortenerOptions)
    {
        // TODO: need to add middleware to handle ValidationExceptions
        return serviceCollection.AddCarter()
                                .AddMediatR(config =>
                                {
                                    config.Lifetime = ServiceLifetime.Transient;
                                    config.RegisterServicesFromAssemblyContaining<Program>();
                                    config.RequestPreProcessorsToRegister.Add(new(typeof(IRequestPreProcessor<>), typeof(ValidationProcessor<>), ServiceLifetime.Transient));
                                })
                                .AddSingleton<ISystemClock, SystemClock>()
                                .AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true)
                                .AddCreateUrlServices(configureUrlShortenerOptions);
    }
}