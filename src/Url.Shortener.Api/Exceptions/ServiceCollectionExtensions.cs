namespace Url.Shortener.Api.Exceptions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExceptionServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddExceptionHandler<DomainExceptionHandler>()
                         .AddProblemDetails();
}