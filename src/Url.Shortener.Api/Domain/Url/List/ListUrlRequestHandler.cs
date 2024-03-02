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
        var queryable = _dbContext.UrlMetadata.OrderByDescending(x => x.CreatedAtUtc);
        var totalCount = await queryable.CountAsync(cancellationToken);
        var pageCount = (int)Math.Ceiling((double)totalCount / request.PageSize);
        var itemsToSkip = (request.Page - 1) * request.PageSize;
        if (totalCount == 0 || itemsToSkip >= totalCount)
        {
            return new()
            {
                Data = Enumerable.Empty<UrlMetadata>(),
                Page = request.Page,
                PageSize = request.PageSize,
                PageCount = pageCount
            };
        }
        
        var items = await queryable.Skip(itemsToSkip)
                                   .Take(request.PageSize)
                                   .MapToUrlMetadataContract()
                                   .ToArrayAsync(cancellationToken);

        return new()
        {
            Data = items,
            Page = request.Page,
            PageSize = request.PageSize,
            PageCount = pageCount
        };
    }
}