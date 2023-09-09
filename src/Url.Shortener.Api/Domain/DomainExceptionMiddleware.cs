using System.Net;
using FluentValidation;
using Url.Shortener.Api.Exceptions;

namespace Url.Shortener.Api.Domain;

internal static class DomainExceptionMiddleware
{
    private const int BadRequestStatusCode = (int)HttpStatusCode.BadRequest;
    private const int NotFoundStatusCode = (int)HttpStatusCode.NotFound;

    public static IApplicationBuilder UseDomainExceptionMiddleware(this IApplicationBuilder webApplication) =>
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
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = NotFoundStatusCode;
                var result = TypedResults.NotFound(notFoundException.Message);
                await context.Response.WriteAsJsonAsync(result);
            }
        });
}