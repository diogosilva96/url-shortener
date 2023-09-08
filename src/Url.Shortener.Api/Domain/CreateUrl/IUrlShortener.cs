namespace Url.Shortener.Api.Domain.CreateUrl;

internal interface IUrlShortener
{
    public string Shorten(string url);
}