namespace Url.Shortener.Data.Models;

public class UrlMetadata
{
    public Guid Id { get; init; }

    public string ShortUrl { get; set; } = string.Empty;
    
    public string FullUrl { get; set; } = string.Empty;
    
    public DateTimeOffset CreatedAtUtc { get; set; }
}