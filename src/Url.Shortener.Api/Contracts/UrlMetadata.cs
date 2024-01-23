namespace Url.Shortener.Api.Contracts;

public class UrlMetadata
{
    public required string ShortUrl { get; init; } = string.Empty;

    public required string FullUrl { get; init; } = string.Empty;

    public required DateTimeOffset CreatedAtUtc { get; init; }
}