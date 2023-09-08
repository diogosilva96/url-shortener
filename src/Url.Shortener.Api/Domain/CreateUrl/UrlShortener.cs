using System.Text;
using Microsoft.Extensions.Options;

namespace Url.Shortener.Api.Domain.CreateUrl;

internal class UrlShortener : IUrlShortener
{
    private readonly char[] _characters;
    private readonly int _urlSize;

    public UrlShortener(IOptions<UrlShortenerOptions> options)
    {
        _characters = options.Value.Characters.ToCharArray();
        _urlSize = options.Value.UrlSize;
    }
    
    public string GenerateUrl()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < _urlSize; i++)
        {
            var characterIndex = Random.Shared.Next(0, _urlSize);
            sb.Append(_characters[characterIndex]);
        }

        return sb.ToString();
    }
}