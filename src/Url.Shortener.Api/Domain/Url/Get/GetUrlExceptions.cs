using Url.Shortener.Api.Exceptions;

namespace Url.Shortener.Api.Domain.Url.Get;

internal static class GetUrlExceptions
{
    public static NotFoundException CodeNotFound() => new("Could not find url metadata for the specified code.");
}