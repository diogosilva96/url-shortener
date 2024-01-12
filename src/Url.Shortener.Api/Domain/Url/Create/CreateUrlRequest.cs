using MediatR;

namespace Url.Shortener.Api.Domain.Url.Create;

internal record CreateUrlRequest(string Url, string? ShortUrl = default) : IRequest<string>, IValidatableRequest
{ }