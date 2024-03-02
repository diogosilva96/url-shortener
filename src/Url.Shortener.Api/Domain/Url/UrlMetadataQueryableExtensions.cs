using Url.Shortener.Api.Contracts;

namespace Url.Shortener.Api.Domain.Url;

public static class UrlMetadataQueryableExtensions
{
    public static IQueryable<UrlMetadata> MapToUrlMetadataContract(this IQueryable<Shortener.Data.Models.UrlMetadata> queryable) =>
        queryable.Select(x => new UrlMetadata
        {
            Code = x.Code,
            FullUrl = x.FullUrl,
            CreatedAtUtc = x.CreatedAtUtc
        });
}