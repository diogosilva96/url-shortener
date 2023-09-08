using MediatR;

namespace Url.Shortener.Api.Domain.CreateUrl;

internal record CreateUrlRequest(string FullUrl) : IRequest<string> { }

