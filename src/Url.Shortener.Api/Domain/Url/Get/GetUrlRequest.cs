using MediatR;

namespace Url.Shortener.Api.Domain.Url.Get;

internal record GetUrlRequest(string ShortUrl) : IRequest<string>, IValidatableRequest
{ }