namespace Url.Shortener.Api.Endpoint;

public interface IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app);
}