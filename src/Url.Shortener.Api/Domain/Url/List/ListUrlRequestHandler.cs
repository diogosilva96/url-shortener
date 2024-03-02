using MediatR;
using Microsoft.EntityFrameworkCore;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Data;

namespace Url.Shortener.Api.Domain.Url.List;

public class ListUrlRequestHandler : IRequestHandler<ListUrlRequest, PagedResult<UrlMetadata>>
{
    private readonly ApplicationDbContext _dbContext;

    public ListUrlRequestHandler(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<PagedResult<UrlMetadata>> Handle(ListUrlRequest request, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.UrlMetadata;
        var totalCount = await queryable.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return new()
            {
                Data = Enumerable.Empty<UrlMetadata>(),
                Page = request.Page,
                PageSize = request.PageSize,
                PageCount = totalCount
            };
        }

        var itemsToSkip = request.PageSize - 1 * request.Page;
        var items = await queryable.Skip(itemsToSkip)
                                   .Take(request.PageSize)
                                   .MapToUrlMetadataContract()
                                   .ToArrayAsync(cancellationToken);

        return new()
        {
            Data = items,
            Page = request.Page,
            PageSize = request.PageSize,
            PageCount = totalCount
        };
    }
}