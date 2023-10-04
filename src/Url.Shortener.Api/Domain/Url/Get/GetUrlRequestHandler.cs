using MediatR;
using Microsoft.EntityFrameworkCore;
using Url.Shortener.Api.Exceptions;
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
        var urlMetadata = await _dbContext.UrlMetadata.FirstOrDefaultAsync(x => x.ShortUrl == request.ShortUrl, cancellationToken);
        // ReSharper disable once InvertIf
        if (urlMetadata == default)
        {
            _logger.LogWarning("Could not find metadata for short url '{ShortUrl}'", request.ShortUrl);
            // might be better to use strongly typed results at the domain level instead - as it has introduces less 'magic'.
            // E.g, we can use minimal api result type Results<Ok<string>, NotFound, BadRequest>>
            throw new NotFoundException("Could not find matching url for the specified short url.");
        }

        return urlMetadata.FullUrl;
    }
}