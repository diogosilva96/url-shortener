using Serilog;
using Url.Shortener.Api.Endpoints;

namespace Url.Shortener.Api;

public static class MiddlewareConfigurator
{
    public static WebApplication ConfigureMiddleware(this WebApplication webApplication)
    {
        webApplication.ConfigureSwagger()
                      .MapEndpoints();

        webApplication.UseExceptionHandler()
                      .UseSerilogRequestLogging()
                      .UseRouting()
                      .UseHsts()
                      .UseHttpsRedirection()
                      .UseAuthorization();

        return webApplication;
    }

    private static WebApplication ConfigureSwagger(this WebApplication webApplication)
    {
        if (!webApplication.Environment.IsDevelopment()) return webApplication;

        webApplication.UseSwagger();
        webApplication.UseSwaggerUI();

        return webApplication;
    }
}