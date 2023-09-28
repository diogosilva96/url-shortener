using Carter;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Url.Shortener.Api.Health;

public class HealthEndpoints : ICarterModule
{
    private static readonly IDictionary<HealthStatus, int> _healthStatusCodeMapper = new Dictionary<HealthStatus, int>
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    };

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("health");

        // TODO: add host filtering? e.g., .RequireHost("*:5001")
        group.MapHealthChecks("live", new()
        {
            ResultStatusCodes = _healthStatusCodeMapper,
            Predicate = check => check.Tags.Contains(HealthTags.Live)
        });

        group.MapHealthChecks("ready", new()
        {
            ResultStatusCodes = _healthStatusCodeMapper,
            Predicate = check => check.Tags.Contains(HealthTags.Ready)
        });
    }
}