using Serilog;

namespace Url.Shortener.Api;

public static class MiddlewareConfigurator
{
    public static WebApplication ConfigureMiddleware(this WebApplication webApplication)
    {
        ConfigureSwagger(webApplication);

        webApplication.UseSerilogRequestLogging()
                      .UseHsts()
                      .UseHttpsRedirection()
                      .UseAuthorization();

        // TODO: remove & use minimal apis
        webApplication.MapControllers();

        return webApplication;
    }

    private static void ConfigureSwagger(WebApplication webApplication)
    {
        if (!webApplication.Environment.IsDevelopment()) return;
        
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI();
    }
}