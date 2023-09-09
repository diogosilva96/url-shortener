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
                      .UseRouting()
                      .UseHsts()
                      .UseHttpsRedirection()
                      .UseAuthorization();

        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseDeveloperExceptionPage();
        }

        webApplication.UseValidationMappingMiddleware();
        
        // TODO: remove
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