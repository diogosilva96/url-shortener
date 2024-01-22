using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.Domain.Url.Redirect;

namespace Url.Shortener.Api.Domain.Url;

public class UrlEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var apiUrlGroup = app.MapGroup("")
                       .WithOpenApi();

        apiUrlGroup.MapPost("api/urls", CreateUrlAsync)
             .WithName("CreateUrl")
             .WithDescription("Creates a short url based on the specified request.")
             .Produces<string>()
             .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest)
             .Produces<ProblemHttpResult>(StatusCodes.Status500InternalServerError);

        app.MapGet("{shortUrl}", RedirectUrlAsync)
           .WithOpenApi()
           .WithName("RedirectUrl")
           .WithDescription("Redirects the request based on the specified short url.")
           .Produces(StatusCodes.Status308PermanentRedirect)
           .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<NotFound>(StatusCodes.Status404NotFound)
           .Produces<ProblemHttpResult>(StatusCodes.Status500InternalServerError);
    }

    public static async Task<IResult> CreateUrlAsync(CreateUrlRequest request, IMediator mediator, CancellationToken cancellationToken = default)
    {
        var domainRequest = new Create.CreateUrlRequest(request.Url, request.ShortUrl);
        var shortUrl = await mediator.Send(domainRequest, cancellationToken);
        return TypedResults.Ok(shortUrl);
    }

    public static async Task<IResult> RedirectUrlAsync(string shortUrl, IMediator mediator, CancellationToken cancellationToken = default)
    {
        var domainRequest = new RedirectUrlRequest(shortUrl);
        var redirectUrl = await mediator.Send(domainRequest, cancellationToken);
        return TypedResults.Redirect(redirectUrl, true, true);
    }
}