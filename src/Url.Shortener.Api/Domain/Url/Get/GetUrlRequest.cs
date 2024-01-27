using Url.Shortener.Api.Contracts;

namespace Url.Shortener.Api.Domain.Url.Get;

internal record GetUrlRequest(string Code) : IValidatableRequest<UrlMetadata>
{ }