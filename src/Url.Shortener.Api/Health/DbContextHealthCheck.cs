using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Url.Shortener.Api.Health;

internal class DbContextHealthCheck<TDbContext> : IHealthCheck where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    private readonly ILogger<DbContextHealthCheck<TDbContext>> _logger;

    public DbContextHealthCheck(TDbContext dbContext, ILogger<DbContextHealthCheck<TDbContext>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Database.ExecuteSqlAsync($"SELECT 1", cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "The {DbContextType}'s database not could not be reached.", typeof(TDbContext).Name);
            return HealthCheckResult.Unhealthy();
        }
    }
}