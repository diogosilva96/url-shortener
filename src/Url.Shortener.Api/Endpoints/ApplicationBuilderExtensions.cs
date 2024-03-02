namespace Url.Shortener.Api.Endpoints;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        foreach (var endpoint in app.Services.GetServices<IEndpoint>())
        {
            endpoint.AddRoutes(app);
        }

        return app;
    }
}