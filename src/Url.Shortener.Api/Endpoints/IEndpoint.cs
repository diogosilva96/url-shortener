namespace Url.Shortener.Api.Endpoints;

public interface IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app);
}