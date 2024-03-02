namespace Url.Shortener.Api.Endpoint;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder MapEndpoints(
        this WebApplication app)
    {
        var endpoints = app.Services
                           .GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.AddRoutes(app);
        }

        return app;
    }
}