using Microsoft.Extensions.Logging;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Redirect;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Data;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Redirect.Builder;

internal class RedirectUrlRequestHandlerBuilder
{
    private ApplicationDbContext _dbContext;
    private ILogger<RedirectUrlRequestHandler> _logger;

    public RedirectUrlRequestHandlerBuilder()
    {
        _dbContext = new UrlShortenerDbContextBuilder().Build();
        _logger = Substitute.For<ILogger<RedirectUrlRequestHandler>>();
    }

    public RedirectUrlRequestHandler Build() => new(_dbContext, _logger);

    public RedirectUrlRequestHandlerBuilder With(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        return this;
    }

    public RedirectUrlRequestHandlerBuilder With(ILogger<RedirectUrlRequestHandler> logger)
    {
        _logger = logger;

        return this;
    }
}