using MediatR;
using Microsoft.EntityFrameworkCore;
using Url.Shortener.Data;
using Url.Shortener.Data.Models;

namespace Url.Shortener.Api.Domain.Url.Create;

internal class CreateUrlRequestHandler : IRequestHandler<CreateUrlRequest, string>
{
    private readonly ICodeGenerator _codeGenerator;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CreateUrlRequestHandler> _logger;
    private readonly TimeProvider _timeProvider;

    public CreateUrlRequestHandler(ApplicationDbContext dbContext,
        ICodeGenerator codeGenerator,
        TimeProvider timeProvider,
        ILogger<CreateUrlRequestHandler> logger)
    {
        _dbContext = dbContext;
        _codeGenerator = codeGenerator;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task<string> Handle(CreateUrlRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Code == default)
        {
            var code = await GenerateCodeAsync(cancellationToken);
            await CreateUrlMetadataAsync(request, code, cancellationToken);
            return code;
        }

        if (await IsCodeAlreadyInUseAsync(request.Code, cancellationToken))
        {
            throw CreateUrlExceptions.CodeAlreadyInUse();
        }

        await CreateUrlMetadataAsync(request, request.Code, cancellationToken);
        return request.Code;
    }

    private async Task CreateUrlMetadataAsync(CreateUrlRequest request, string code, CancellationToken cancellationToken = default)
    {
        var urlMetadata = new UrlMetadata
        {
            Id = Guid.NewGuid(),
            FullUrl = request.Url,
            CreatedAtUtc = _timeProvider.GetUtcNow(),
            Code = code
        };

        _dbContext.UrlMetadata.Add(urlMetadata);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<string> GenerateCodeAsync(CancellationToken cancellationToken)
    {
        var generatedCode = string.Empty;
        var isValidUrl = false;

        // TODO: this loop can be optimized, we have a few options:
        // - We could simply fail fast by returning internal server error (500) if the url cannot be generated, so that the api consumers can retry at a later time
        // - We could simply insert into db and check for unique constraint exception.
        // - We could generate a bunch of codes and then checks which one does not exist in the db (with a single network call)
        var retryCount = 0;
        while (!isValidUrl)
        {
            generatedCode = _codeGenerator.GenerateCode();

            if (await IsCodeAlreadyInUseAsync(generatedCode, cancellationToken))
            {
                _logger.LogWarning("The code '{Code}' is already in use (retry count: {RetryCount}).", generatedCode, retryCount++);
                continue;
            }

            isValidUrl = true;
        }

        _logger.LogInformation("The code '{Code}' was successfully generated after {RetryCount} retries.", generatedCode,
            retryCount);

        return generatedCode;
    }

    private async Task<bool> IsCodeAlreadyInUseAsync(string code, CancellationToken cancellationToken) =>
        await _dbContext.UrlMetadata.AnyAsync(x => x.Code == code, cancellationToken);
}