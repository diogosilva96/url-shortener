namespace Url.Shortener.Api.Domain.Url.Redirect;

internal class CacheKeys
{
    public static string RedirectUrl(string code) => $"Url-Redirect-{code}";
}