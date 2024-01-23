namespace Url.Shortener.Api.Domain.Url.Create;

internal record CreateUrlRequest(string Url, string? ShortUrl = default) : IValidatableRequest<string>
{ }