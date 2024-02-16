using MediatR;
using Url.Shortener.Api.Contracts;

namespace Url.Shortener.Api.Domain.Url.List;

public class ListUrlRequestHandler : IRequestHandler<ListUrlRequest, PagedResult<UrlMetadata>>
{
    public Task<PagedResult<UrlMetadata>> Handle(ListUrlRequest request, CancellationToken cancellationToken) => throw new NotImplementedException();
}