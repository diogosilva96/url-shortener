using Url.Shortener.Api.Exceptions;

namespace Url.Shortener.Api.Domain.Url.Redirect;

internal static class RedirectUrlExceptions
{
    public static NotFoundException CodeNotFound() => new("Could not find matching url for the specified code.");
}