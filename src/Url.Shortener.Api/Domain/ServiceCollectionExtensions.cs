using Carter;
using FluentValidation;

namespace Url.Shortener.Api.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddCarter()
                                .AddMediatR(config => config.Lifetime = ServiceLifetime.Transient)
                                .AddValidatorsFromAssembly(typeof(Program).Assembly);
    }
}