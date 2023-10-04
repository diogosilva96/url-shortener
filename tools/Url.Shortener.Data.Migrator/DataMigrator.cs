using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Url.Shortener.Data.Migrator;

public class DataMigrator : BackgroundService
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ILogger<DataMigrator> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DataMigrator(IHostApplicationLifetime applicationLifetime,
        IServiceProvider serviceProvider,
        ILogger<DataMigrator> logger)
    {
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await ApplyDatabaseMigrations(dbContext, stoppingToken);

        await StopAsync(stoppingToken);
    }

    private async Task ApplyDatabaseMigrations(DbContext dbContext, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting data migration...");

        await dbContext.Database.MigrateAsync(cancellationToken);

        _logger.LogInformation("Data migration completed.");
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Data Migrator started...");
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        _applicationLifetime.StopApplication();
    }
}