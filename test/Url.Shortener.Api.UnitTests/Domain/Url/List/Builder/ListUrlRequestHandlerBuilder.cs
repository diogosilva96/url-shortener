using Url.Shortener.Api.Domain.Url.List;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Data;

namespace Url.Shortener.Api.UnitTests.Domain.Url.List.Builder;

public class ListUrlRequestHandlerBuilder
{
    private ApplicationDbContext _dbContext;


    public ListUrlRequestHandlerBuilder() => _dbContext = new UrlShortenerDbContextBuilder().Build();

    public ListUrlRequestHandler Build() => new(_dbContext);

    public ListUrlRequestHandlerBuilder With(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        return this;
    }
}