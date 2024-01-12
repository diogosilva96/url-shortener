namespace Url.Shortener.Api.Contracts;

public class CreateUrlRequest
{
    public required string Url { get; init; }

    public string? ShortUrl { get; init; }
}