using System.Net;
using FluentValidation;

namespace Url.Shortener.Api.Domain;

internal static class ValidationMappingMiddleware
{
    private const int BadRequestStatusCode = (int)HttpStatusCode.BadRequest;

    public static IApplicationBuilder UseValidationMappingMiddleware(this IApplicationBuilder webApplication) =>
        webApplication.Use(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (ValidationException validationException)
            {
                context.Response.StatusCode = BadRequestStatusCode;

                var errors = validationException.Errors
                                                .GroupBy(x => x.PropertyName)
                                                .ToDictionary(group => group.Key, group => group.Select(x => x.ErrorMessage).ToArray());

                var result = new HttpValidationProblemDetails(errors)
                {
                    Status = BadRequestStatusCode
                };

                await context.Response.WriteAsJsonAsync(result);
            }
        });
}