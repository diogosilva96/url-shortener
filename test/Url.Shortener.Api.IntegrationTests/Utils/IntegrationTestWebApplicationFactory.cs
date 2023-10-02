using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using Url.Shortener.Data;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Utils;

public sealed class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private IServiceScope? _scope;

    public IntegrationTestWebApplicationFactory() =>
        _dbContainer = new PostgreSqlBuilder()
                       .WithImage("postgres:latest")
                       .WithDatabase("UrlShortener")
                       .WithUsername("postgres")
                       .WithPassword("postgres")
                       .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<UrlShortenerDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    } 

    public new Task DisposeAsync() => _dbContainer.StopAsync();

    public T GetRequiredService<T>() where T : notnull
    {
        _scope ??= Services.CreateScope();

        return _scope.ServiceProvider.GetRequiredService<T>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment(Environments.Development)
               .ConfigureLogging(ConfigureIntegrationTestLogging)
               .ConfigureTestServices(services =>
               {
                   ReplaceEntityFrameworkServices(services);
                   ReplaceExternalServicesWithSubstitutes(services);
               });
    }

    private void ConfigureIntegrationTestLogging(ILoggingBuilder builder)
    {
        builder.ClearProviders();

        // TODO: find a way to inject ITestOutputHelper
        // var logger = new LoggerConfiguration().MinimumLevel.Information()
        //                                       .WriteTo.TestOutput(_testOutputHelper, 
        //                                           outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}] [{Version}] {SourceContext} [{EventId}]: {Message:lj}{NewLine}{Exception}")
        //                                       .CreateLogger();
        //
        // builder.AddSerilog(logger);
    }

    private void ReplaceEntityFrameworkServices(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<UrlShortenerDbContext>));

        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }

        services.AddDbContext<UrlShortenerDbContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString(), 
                                                                           x => x.MigrationsAssembly("Url.Shortener.Data.Migrator"))
                                                                       .UseSnakeCaseNamingConvention());
    }


    private static void ReplaceExternalServicesWithSubstitutes(IServiceCollection services)
    { }

    public void SeedData(Action<UrlShortenerDbContext> seedDelegate)
    {
        using var scope = Services.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<UrlShortenerDbContext>();

        seedDelegate.Invoke(dbContext);

        dbContext.SaveChanges();
    }
}