using Microsoft.Extensions.Logging;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Data;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Get.Builder;

internal class GetUrlRequestHandlerBuilder
{
    private ApplicationDbContext _dbContext;
    private ILogger<GetUrlRequestHandler> _logger;
    

    public GetUrlRequestHandlerBuilder()
    {
        _dbContext = new UrlShortenerDbContextBuilder().Build();
        _logger = Substitute.For<ILogger<GetUrlRequestHandler>>();
    }

    public GetUrlRequestHandler Build() => new(_dbContext, _logger);

    public GetUrlRequestHandlerBuilder With(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        return this;
    }
    
    public GetUrlRequestHandlerBuilder With(ILogger<GetUrlRequestHandler> logger)
    {
        _logger = logger;

        return this;
    }
}