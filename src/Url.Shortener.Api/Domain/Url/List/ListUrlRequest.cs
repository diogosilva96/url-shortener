using MediatR;
using Url.Shortener.Api.Contracts;

namespace Url.Shortener.Api.Domain.Url.List;

public record ListUrlRequest(int PageSize, int Page) : IRequest<PagedResult<UrlMetadata>>;