using Carter;
using FluentValidation;
using MediatR;
using Serilog;
using Url.Shortener.Api.Domain;
using Url.Shortener.Api.Domain.CreateUrl;

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
                      .UseAuthorization()
                      .UseValidationMappingMiddleware();

        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseDeveloperExceptionPage();
        }

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