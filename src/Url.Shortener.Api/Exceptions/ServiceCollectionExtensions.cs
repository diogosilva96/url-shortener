namespace Url.Shortener.Api.Exceptions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExceptionServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddExceptionHandler<DomainExceptionHandler>()
                         .AddProblemDetails();
}