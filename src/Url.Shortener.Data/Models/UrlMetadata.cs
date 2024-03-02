namespace Url.Shortener.Data.Models;

public class UrlMetadata
{
    public required Guid Id { get; init; }

    public required string Code { get; set; } = string.Empty;

    public required string FullUrl { get; set; } = string.Empty;

    public required DateTimeOffset CreatedAtUtc { get; init; }
}