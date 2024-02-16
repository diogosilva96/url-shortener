using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Url.Shortener.Api.Exceptions;

public class DomainExceptionHandler : IExceptionHandler
{
    private const int BadRequestStatusCode = StatusCodes.Status400BadRequest;
    private const int NotFoundStatusCode = StatusCodes.Status404NotFound;
    private readonly ILogger<DomainExceptionHandler> _logger;

    public DomainExceptionHandler(ILogger<DomainExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        switch (exception)
        {
            case ValidationException validationException:
            {
                httpContext.Response.StatusCode = BadRequestStatusCode;

                var errors = validationException.Errors
                                                .GroupBy(x => x.PropertyName)
                                                .ToDictionary(group => group.Key, group => group.Select(x => x.ErrorMessage).ToArray());

                var result = new HttpValidationProblemDetails(errors)
                {
                    Status = BadRequestStatusCode
                };

                await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

                return true;
            }
            case NotFoundException notFoundException:
            {
                httpContext.Response.StatusCode = NotFoundStatusCode;
                var result = TypedResults.NotFound(notFoundException.Message);
                await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
                return true;
            }
            default:
                return false;
        }
    }
}