using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Url.Shortener.Data;
using Url.Shortener.Data.Models;

namespace Url.Shortener.Api.Domain.Url.Create;

internal class CreateUrlRequestHandler : IRequestHandler<CreateUrlRequest, string>
{
    private readonly ISystemClock _clock;
    private readonly UrlShortenerDbContext _dbContext;
    private readonly ILogger<CreateUrlRequestHandler> _logger;
    private readonly IUrlShortener _urlShortener;

    public CreateUrlRequestHandler(UrlShortenerDbContext dbContext,
        IUrlShortener urlShortener,
        ISystemClock clock,
        ILogger<CreateUrlRequestHandler> logger)
    {
        _dbContext = dbContext;
        _urlShortener = urlShortener;
        _clock = clock;
        _logger = logger;
    }

    public async Task<string> Handle(CreateUrlRequest request, CancellationToken cancellationToken = default)
    {
        var shortenedUrl = await GenerateShortenedUrlAsync(cancellationToken);

        await CreateUrlMetadataAsync(request, shortenedUrl, cancellationToken);

        return shortenedUrl;
    }

    private async Task CreateUrlMetadataAsync(CreateUrlRequest request, string shortenedUrl, CancellationToken cancellationToken = default)
    {
        var urlMetadata = new UrlMetadata
        {
            Id = Guid.NewGuid(),
            FullUrl = request.Url,
            CreatedAtUtc = _clock.UtcNow,
            ShortUrl = shortenedUrl
        };

        _dbContext.UrlMetadata.Add(urlMetadata);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<string> GenerateShortenedUrlAsync(CancellationToken cancellationToken)
    {
        var generatedUrl = string.Empty;
        var isValidUrl = false;

        // TODO: this loop can be optimized, alternatively we could simply insert into db and check for unique constraint exception.
        var retryCount = 0;
        while (!isValidUrl)
        {
            generatedUrl = _urlShortener.GenerateUrl();

            if (await IsUrlAlreadyInUseAsync(generatedUrl, cancellationToken))
            {
                _logger.LogWarning("The short url '{ShortUrl}' is already in use (retry count: {RetryCount}).", generatedUrl, retryCount++);
                continue;
            }

            isValidUrl = true;
        }

        _logger.LogInformation("The short url '{ShortUrl}' was successfully generated after {RetryCount} retries.", generatedUrl,
            retryCount);

        return generatedUrl;
    }

    private async Task<bool> IsUrlAlreadyInUseAsync(string generatedUrl, CancellationToken cancellationToken) =>
        await _dbContext.UrlMetadata.AnyAsync(x => x.ShortUrl == generatedUrl, cancellationToken);
}