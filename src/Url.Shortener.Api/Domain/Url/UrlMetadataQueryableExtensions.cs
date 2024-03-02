using Url.Shortener.Data.Models;

namespace Url.Shortener.Api.Domain.Url;

public static class UrlMetadataQueryableExtensions
{
    public static IQueryable<Contracts.UrlMetadata> MapToUrlMetadataContract(this IQueryable<UrlMetadata> queryable) => 
        queryable.Select(x => new Contracts.UrlMetadata()
        {
            Code = x.Code,
            FullUrl = x.FullUrl,
            CreatedAtUtc = x.CreatedAtUtc
        });
}