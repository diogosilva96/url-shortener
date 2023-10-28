namespace Url.Shortener.Api.Domain.Url.Get;

internal class CacheKeys
{
    public static string GetUrl(string url) => $"Url-Get-{url}";
}