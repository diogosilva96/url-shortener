using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Url.Shortener.Data;
using Url.Shortener.Data.Models;

namespace Url.Shortener.Api.UnitTests.Data.Builder;

public class UrlShortenerDbContextBuilder
{
    private IEnumerable<UrlMetadata> _urlMetadata = BuildDbSet(Array.Empty<UrlMetadata>());

    public ApplicationDbContext Build()
    {
        var dbContext = Substitute.For<ApplicationDbContext>();
        dbContext.UrlMetadata.Returns(_urlMetadata);
        return dbContext;
    }

    public UrlShortenerDbContextBuilder With(IEnumerable<UrlMetadata> urlMetadata)
    {
        _urlMetadata = BuildDbSet(urlMetadata);

        return this;
    }

    private static DbSet<T> BuildDbSet<T>(IEnumerable<T> data) where T : class
        => data.AsQueryable()
               .BuildMockDbSet();
}