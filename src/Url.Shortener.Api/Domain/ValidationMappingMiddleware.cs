using System.Net;
using FluentValidation;

namespace Url.Shortener.Api.Domain;

public class ValidationMappingMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMappingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException validationException)
        {
            context.Response.StatusCode = BadRequestStatusCode;
            var errors = validationException.Errors
                                            .GroupBy(x => x.PropertyName)
                                            .ToDictionary(group => group.Key, group => group.Select(x => x.ErrorMessage).ToArray());
            var result = Results.ValidationProblem(errors);

            await context.Response.WriteAsJsonAsync(result);
        }
    }

    private const int BadRequestStatusCode = (int)HttpStatusCode.BadRequest;
}