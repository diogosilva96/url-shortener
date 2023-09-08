using Carter;
using Serilog;
using Url.Shortener.Api.Domain;

namespace Url.Shortener.Api;

public static class MiddlewareConfigurator
{
    public static WebApplication ConfigureMiddleware(this WebApplication webApplication)
    {
        webApplication.ConfigureSwagger()
                      .MapCarter();

        webApplication.UseSerilogRequestLogging()
                      .UseHsts()
                      .UseHttpsRedirection()
                      .UseAuthorization();

        webApplication.UseMiddleware<ValidationMappingMiddleware>();

        // TODO: remove & use minimal apis
        webApplication.MapControllers();

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