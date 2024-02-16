namespace Url.Shortener.Api.Domain.Url.Create;

public record CreateUrlRequest(string Url, string? Code = default) : IValidatableRequest<string>
{ }