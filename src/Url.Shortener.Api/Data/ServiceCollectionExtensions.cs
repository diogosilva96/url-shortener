using Microsoft.EntityFrameworkCore;
using Url.Shortener.Data;

namespace Url.Shortener.Api.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection serviceCollection, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("The connection string should be specified.", nameof(connectionString));
        
        return serviceCollection.AddDbContext<UrlShortenerDbContext>
        (
            options => options.UseNpgsql(connectionString, 
            x => x.MigrationsAssembly("Url.Shortener.Data.Migrator"))
        );
    }
}