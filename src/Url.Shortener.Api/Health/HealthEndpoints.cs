using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Url.Shortener.Api.Contracts;

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
        // TODO: add host filtering? e.g., .RequireHost("*:5001")
        var group = app.MapGroup(Urls.Health.BasePath);

        group.MapHealthChecks(string.Empty, CreateHealthCheckOptions());

        group.MapHealthChecks("live", CreateHealthCheckOptions(check => check.Tags.Contains(HealthTags.Live)));

        group.MapHealthChecks("ready", CreateHealthCheckOptions(check => check.Tags.Contains(HealthTags.Ready)));
    }

    private static HealthCheckOptions CreateHealthCheckOptions(Func<HealthCheckRegistration, bool>? healthCheckPredicate = default) =>
        new()
        {
            ResultStatusCodes = _healthStatusCodeMapper,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            Predicate = healthCheckPredicate
        };
}