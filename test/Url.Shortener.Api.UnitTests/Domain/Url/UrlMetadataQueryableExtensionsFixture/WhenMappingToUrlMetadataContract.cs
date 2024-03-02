using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Data.Models;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.UrlMetadataQueryableExtensionsFixture;

public class WhenMappingToUrlMetadataContract
{
    private readonly IQueryable<UrlMetadata> _urlMetadata;
    
    public WhenMappingToUrlMetadataContract()
    {
        _urlMetadata = new[]
        {
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build()
        }.AsQueryable();
    }

    [Fact]
    public void ThenNoExceptionIsThrown()
    {
        var exception = Record.Exception(WhenMapping);
        
        Assert.Null(exception);
    }
    
    [Fact]
    public void ThenTheUrlMetadataIsMapped()
    {
        var result = WhenMapping();
        
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public void ThenTheExpectedNumberOfUrlMetadataIsMapped()
    {
        var result = WhenMapping();

        Assert.Equal(_urlMetadata.Count(), result.Length);
    }
    
    [Fact]
    public void ThenTheExpectedUrlMetadataIsMapped()
    {
        var result = WhenMapping();

        Assert.All(_urlMetadata, expectedMetadata => Assert.Single(result, x => x.Code == expectedMetadata.Code && 
                                                                                x.FullUrl == expectedMetadata.FullUrl && 
                                                                                x.CreatedAtUtc == expectedMetadata.CreatedAtUtc));
    }
    private Contracts.UrlMetadata[] WhenMapping() => _urlMetadata.MapToUrlMetadataContract().ToArray();
}