namespace Url.Shortener.Data.Models;

public class UrlMetadata
{
    public required Guid Id { get; init; }

    public required string ShortUrl { get; set; } = string.Empty;

    public required string FullUrl { get; set; } = string.Empty;

    public required DateTimeOffset CreatedAtUtc { get; set; }
}