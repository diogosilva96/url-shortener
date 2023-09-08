using System.ComponentModel.DataAnnotations;

namespace Url.Shortener.Api.Domain.CreateUrl;

internal class UrlShortenerOptions
{
    [Required]
    public string Characters { get; set; } = string.Empty;

    [Required]
    public int UrlSize { get; set; } = DefaultUrlSize;

    private const int DefaultUrlSize = 10;
}