namespace Url.Shortener.Api.Domain.Url.Redirect;

internal class CacheKeys
{
    public static string RedirectUrl(string url) => $"Url-Redirect-{url}";
}