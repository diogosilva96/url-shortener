using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Data;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;

internal class CreateUrlRequestHandlerBuilder
{
    private ISystemClock _clock;
    private UrlShortenerDbContext _dbContext;
    private ILogger<CreateUrlRequestHandler> _logger;
    private IUrlShortener _urlShortener;

    public CreateUrlRequestHandlerBuilder()
    {
        _dbContext = new UrlShortenerDbContextBuilder().Build();
        _urlShortener = Substitute.For<IUrlShortener>();
        _clock = Substitute.For<ISystemClock>();
        _logger = Substitute.For<ILogger<CreateUrlRequestHandler>>();
    }

    public CreateUrlRequestHandler Build() => new(_dbContext, _urlShortener, _clock, _logger);

    public CreateUrlRequestHandlerBuilder With(UrlShortenerDbContext dbContext)
    {
        _dbContext = dbContext;

        return this;
    }

    public CreateUrlRequestHandlerBuilder With(IUrlShortener urlShortener)
    {
        _urlShortener = urlShortener;

        return this;
    }

    public CreateUrlRequestHandlerBuilder With(ISystemClock clock)
    {
        _clock = clock;

        return this;
    }

    public CreateUrlRequestHandlerBuilder With(ILogger<CreateUrlRequestHandler> logger)
    {
        _logger = logger;

        return this;
    }
}