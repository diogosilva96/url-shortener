using Url.Shortener.Api.Exceptions;

namespace Url.Shortener.Api.Domain.Url.Redirect;

internal static class RedirectUrlExceptions
{
    public static NotFoundException UrlNotFound() => new("Could not find matching url for the specified short url.");
}