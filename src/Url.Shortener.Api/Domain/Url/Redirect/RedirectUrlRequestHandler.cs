using MediatR;
using Microsoft.EntityFrameworkCore;
using Url.Shortener.Data;

namespace Url.Shortener.Api.Domain.Url.Redirect;

internal class RedirectUrlRequestHandler : IRequestHandler<RedirectUrlRequest, string>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<RedirectUrlRequestHandler> _logger;

    public RedirectUrlRequestHandler(ApplicationDbContext dbContext, ILogger<RedirectUrlRequestHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> Handle(RedirectUrlRequest request, CancellationToken cancellationToken = default)
    {
        var fullUrl = await _dbContext.UrlMetadata
                                      .Where(x => x.Code == request.Code)
                                      .Select(x => x.FullUrl)
                                      .FirstOrDefaultAsync(cancellationToken);

        // ReSharper disable once InvertIf
        if (string.IsNullOrWhiteSpace(fullUrl))
        {
            _logger.LogWarning("Could not find metadata for code '{Code}'", request.Code);
            throw RedirectUrlExceptions.CodeNotFound();
        }

        return fullUrl;
    }
}