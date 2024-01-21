using Microsoft.Extensions.Logging;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Data;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;

internal class CreateUrlRequestHandlerBuilder
{
    private ApplicationDbContext _dbContext;
    private ILogger<CreateUrlRequestHandler> _logger;
    private TimeProvider _timeProvider;
    private IUrlShortener _urlShortener;

    public CreateUrlRequestHandlerBuilder()
    {
        _dbContext = new UrlShortenerDbContextBuilder().Build();
        _urlShortener = Substitute.For<IUrlShortener>();
        _timeProvider = Substitute.For<TimeProvider>();
        _logger = Substitute.For<ILogger<CreateUrlRequestHandler>>();
    }

    public CreateUrlRequestHandler Build() => new(_dbContext, _urlShortener, _timeProvider, _logger);

    public CreateUrlRequestHandlerBuilder With(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        return this;
    }

    public CreateUrlRequestHandlerBuilder With(IUrlShortener urlShortener)
    {
        _urlShortener = urlShortener;

        return this;
    }

    public CreateUrlRequestHandlerBuilder With(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;

        return this;
    }

    public CreateUrlRequestHandlerBuilder With(ILogger<CreateUrlRequestHandler> logger)
    {
        _logger = logger;

        return this;
    }
}