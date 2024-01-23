using MediatR;
using Microsoft.EntityFrameworkCore;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Data;

namespace Url.Shortener.Api.Domain.Url.Get;

internal class GetUrlRequestHandler : IRequestHandler<GetUrlRequest, UrlMetadata>
{
    private readonly ApplicationDbContext _dbContext;
    
    public GetUrlRequestHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UrlMetadata> Handle(GetUrlRequest request, CancellationToken cancellationToken)
    {
        // TODO: make url metadata fetch reusable
        var metadata = await _dbContext.UrlMetadata
                                       .Where(x => x.ShortUrl == request.ShortUrl)
                                       .Select(x => new UrlMetadata() 
                                       { 
                                           FullUrl = x.FullUrl, 
                                           ShortUrl = x.ShortUrl, 
                                           CreatedAtUtc = x.CreatedAtUtc 
                                       })
                                       .SingleOrDefaultAsync(cancellationToken);

        if (metadata is null)
        {
            throw GetUrlExceptions.UrlNotFound();
        }

        return metadata;
    }
}