using System.ComponentModel.DataAnnotations;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.Domain.Url.Get;

namespace Url.Shortener.Api.Domain.Url;

public class UrlEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/urls")
                       .WithOpenApi();

        group.MapPost("", async (CreateUrlRequest request, IMediator mediator, CancellationToken cancellationToken) =>
             {
                 var domainRequest = new Create.CreateUrlRequest(request.Url);
                 TypedResults.Ok(await mediator.Send(domainRequest, cancellationToken));
             })
             .WithName("CreateUrl")
             .WithDescription("Creates a short url based on the specified request.")
             .Produces<string>()
             .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest)
             .Produces<ProblemHttpResult>(StatusCodes.Status500InternalServerError);

        group.MapGet("{shortUrl}", async ([Required] string shortUrl, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var domainRequest = new GetUrlRequest(shortUrl);
            var redirectUrl = await mediator.Send(domainRequest, cancellationToken);
            return TypedResults.Redirect(redirectUrl, permanent: true);
        })
        .WithName("GetUrl")
        .WithDescription("Redirects the request based on the specified short url.")
        .Produces(StatusCodes.Status308PermanentRedirect)
        .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<NotFound>(StatusCodes.Status404NotFound)
        .Produces<ProblemHttpResult>(StatusCodes.Status500InternalServerError);
    }
}