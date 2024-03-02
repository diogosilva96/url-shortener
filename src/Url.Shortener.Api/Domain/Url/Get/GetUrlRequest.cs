using Url.Shortener.Api.Contracts;

namespace Url.Shortener.Api.Domain.Url.Get;

public record GetUrlRequest(string Code) : IValidatableRequest<UrlMetadata>
{ }