using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Url.Shortener.Api.Contracts;

namespace Url.Shortener.Api.Domain.Url;

public class UrlEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/urls");

        group.MapPost("", async (CreateUrlRequest request, IMediator mediator) =>
             {
                 var domainRequest = new Create.CreateUrlRequest(request.Url);
                 return await mediator.Send(domainRequest);
             })
             .WithName("CreateUrl")
             .Produces<string>()
             .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest)
             .Produces<ProblemHttpResult>(StatusCodes.Status500InternalServerError);
    }
}