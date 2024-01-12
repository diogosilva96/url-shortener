using MediatR;
using Microsoft.EntityFrameworkCore;
using Url.Shortener.Data;

namespace Url.Shortener.Api.Domain.Url.Get;

internal class GetUrlRequestHandler : IRequestHandler<GetUrlRequest, string>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetUrlRequestHandler> _logger;

    public GetUrlRequestHandler(ApplicationDbContext dbContext, ILogger<GetUrlRequestHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> Handle(GetUrlRequest request, CancellationToken cancellationToken = default)
    {
        var fullUrl = await _dbContext.UrlMetadata
                                      .Where(x => x.ShortUrl == request.ShortUrl)
                                      .Select(x => x.FullUrl)
                                      .FirstOrDefaultAsync(cancellationToken);

        // ReSharper disable once InvertIf
        if (string.IsNullOrWhiteSpace(fullUrl))
        {
            _logger.LogWarning("Could not find metadata for short url '{ShortUrl}'", request.ShortUrl);
            throw GetUrlExceptions.UrlNotFound();
        }

        return fullUrl;
    }
}