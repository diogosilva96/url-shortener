using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Url.Shortener.Api.Endpoint;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(
        this IServiceCollection serviceCollection,
        Assembly assembly)
    {
        var serviceDescriptors = assembly
                                 .DefinedTypes
                                 .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
                                 .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                                 .ToArray();

        serviceCollection.TryAddEnumerable(serviceDescriptors);
        return serviceCollection;
    }
}