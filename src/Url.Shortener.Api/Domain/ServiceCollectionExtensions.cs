using Carter;
using FluentValidation;
using Microsoft.Extensions.Internal;
using Url.Shortener.Api.Domain.CreateUrl;

namespace Url.Shortener.Api.Domain;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection, Action<UrlShortenerOptions> configureUrlShortenerOptions)
    {
        return serviceCollection.AddCarter()
                                .AddMediatR(config =>
                                {
                                    config.Lifetime = ServiceLifetime.Transient;
                                    config.AddOpenBehavior(typeof(ValidationPipelineBehaviour<,>));
                                })
                                .AddSingleton<ISystemClock, SystemClock>()
                                .AddValidatorsFromAssembly(typeof(Program).Assembly)
                                .AddCreateUrlServices(configureUrlShortenerOptions);
    }
}