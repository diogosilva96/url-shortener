using System.ComponentModel.DataAnnotations;

namespace Url.Shortener.Api.Domain.Url.Create;

internal class UrlShortenerOptions
{
    private const int DefaultUrlSize = 10;

    [Required]
    public string Characters { get; set; } = string.Empty;

    [Required]
    [Range(5, 50)]
    public int UrlSize { get; set; } = DefaultUrlSize;
}