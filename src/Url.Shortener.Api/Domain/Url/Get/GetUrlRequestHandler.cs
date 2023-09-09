using MediatR;
using Microsoft.EntityFrameworkCore;
using Url.Shortener.Data;

namespace Url.Shortener.Api.Domain.Url.Get;

// Todo: add strongly typed result (e.g., OneOf)
internal class GetUrlRequestHandler : IRequestHandler<GetUrlRequest, string>
{
    private readonly UrlShortenerDbContext _dbContext;
    private readonly ILogger<GetUrlRequestHandler> _logger;

    public GetUrlRequestHandler(UrlShortenerDbContext dbContext, ILogger<GetUrlRequestHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> Handle(GetUrlRequest request, CancellationToken cancellationToken)
    {
        var urlMetadata = await _dbContext.UrlMetadata.FirstOrDefaultAsync(x => x.ShortUrl == request.ShortUrl, cancellationToken);
        if (urlMetadata == default)
        {
            // todo throw not found exception (or add strongly typed results)
            _logger.LogWarning("Could not find metadata for short url '{ShortUrl}'", request.ShortUrl);
            throw new InvalidOperationException("Could not find url metadata");
        }

        return urlMetadata.FullUrl;
    }
}