using Carter;
using MediatR;
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
        }).WithOpenApi();
    }
}