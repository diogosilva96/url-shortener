namespace Url.Shortener.Api.Domain.Url.Redirect;

public class CacheKeys
{
    public static string RedirectUrl(string code) => $"Url-Redirect-{code}";
}