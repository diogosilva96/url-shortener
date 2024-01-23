using MediatR;
using Microsoft.EntityFrameworkCore;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Data;

namespace Url.Shortener.Api.Domain.Url.Get;

internal class GetUrlRequestHandler : IRequestHandler<GetUrlRequest, UrlMetadata>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetUrlRequestHandler> _logger;

    public GetUrlRequestHandler(ApplicationDbContext dbContext, ILogger<GetUrlRequestHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<UrlMetadata> Handle(GetUrlRequest request, CancellationToken cancellationToken)
    {
        // TODO: make url metadata fetch reusable 
        // TODO: add caching
        var metadata = await _dbContext.UrlMetadata
                                       .Where(x => x.ShortUrl == request.ShortUrl)
                                       .Select(x => new UrlMetadata() 
                                       { 
                                           FullUrl = x.FullUrl, 
                                           ShortUrl = x.ShortUrl, 
                                           CreatedAtUtc = x.CreatedAtUtc 
                                       })
                                       .SingleOrDefaultAsync(cancellationToken);

        // ReSharper disable once InvertIf
        if (metadata is null)
        {
            _logger.LogWarning("Could not find url metadata for short url '{ShortUrl}'", request.ShortUrl);
            throw GetUrlExceptions.UrlNotFound();
        }

        return metadata;
    }
}