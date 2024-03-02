using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.Domain.Url.Redirect;
using Url.Shortener.Api.Endpoints;
using ListUrlRequest = Url.Shortener.Api.Domain.Url.List.ListUrlRequest;

namespace Url.Shortener.Api.Domain.Url;

public class UrlEndpoints : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var apiUrlGroup = app.MapGroup(Urls.Api.Urls.BasePath)
                             .WithOpenApi();

        apiUrlGroup.MapGet("{code}", GetUrlAsync)
                   .WithName("GetUrl")
                   .WithDescription("Retrieves the url metadata based on the specified short url.")
                   .Produces<UrlMetadata>()
                   .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest)
                   .Produces<NotFound>(StatusCodes.Status404NotFound)
                   .Produces<ProblemHttpResult>(StatusCodes.Status500InternalServerError);

        apiUrlGroup.MapGet(string.Empty, ListUrlsAsync)
                   .WithName("ListUrls")
                   .WithDescription("Lists the url metadata.")
                   .Produces<UrlMetadata>()
                   .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest)
                   .Produces<ProblemHttpResult>(StatusCodes.Status500InternalServerError);

        apiUrlGroup.MapPost(string.Empty, CreateUrlAsync)
                   .WithName("CreateUrl")
                   .WithDescription("Creates a short url based on the specified request.")
                   .Produces<string>()
                   .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest)
                   .Produces<ProblemHttpResult>(StatusCodes.Status500InternalServerError);

        app.MapGet("{code}", RedirectUrlAsync)
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
        var domainRequest = new Create.CreateUrlRequest(request.Url, request.Code);
        var code = await mediator.Send(domainRequest, cancellationToken);
        return TypedResults.Ok(code);
    }

    public static async Task<IResult> GetUrlAsync(string code, IMediator mediator, CancellationToken cancellationToken = default)
    {
        var domainRequest = new GetUrlRequest(code);
        var metadata = await mediator.Send(domainRequest, cancellationToken);
        return TypedResults.Ok(metadata);
    }

    public static async Task<IResult> ListUrlsAsync(IMediator mediator,
        [FromQuery] int? pageSize = default,
        [FromQuery] int? page = default,
        CancellationToken cancellationToken = default)
    {
        var domainRequest = new ListUrlRequest(pageSize ?? 50, page ?? 1);
        var metadata = await mediator.Send(domainRequest, cancellationToken);
        return TypedResults.Ok(metadata);
    }


    public static async Task<IResult> RedirectUrlAsync(string code, IMediator mediator, CancellationToken cancellationToken = default)
    {
        var domainRequest = new RedirectUrlRequest(code);
        var redirectUrl = await mediator.Send(domainRequest, cancellationToken);
        return TypedResults.Redirect(redirectUrl, true, true);
    }
}