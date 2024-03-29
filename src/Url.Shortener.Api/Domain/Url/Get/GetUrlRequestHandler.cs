﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Data;

namespace Url.Shortener.Api.Domain.Url.Get;

public class GetUrlRequestHandler : IRequestHandler<GetUrlRequest, UrlMetadata>
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
        var metadata = await _dbContext.UrlMetadata
                                       .Where(x => x.Code == request.Code)
                                       .MapToUrlMetadataContract()
                                       .SingleOrDefaultAsync(cancellationToken);

        // ReSharper disable once InvertIf
        if (metadata is null)
        {
            _logger.LogWarning("Could not find url metadata for code '{Code}'", request.Code);
            throw GetUrlExceptions.CodeNotFound();
        }

        return metadata;
    }
}