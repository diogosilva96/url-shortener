using System.ComponentModel.DataAnnotations;
using Url.Shortener.Data.Configuration;

namespace Url.Shortener.Api.Domain.Url.Create;

public class CodeGeneratorOptions
{
    private const int DefaultUrlSize = 10;

    [Required]
    public string Characters { get; set; } = string.Empty;

    [Required]
    [Range(5, Constants.MaxCodeLength)]
    public int UrlSize { get; set; } = DefaultUrlSize;
}