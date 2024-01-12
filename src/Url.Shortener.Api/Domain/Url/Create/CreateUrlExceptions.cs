using FluentValidation;
using FluentValidation.Results;

namespace Url.Shortener.Api.Domain.Url.Create;

internal static class CreateUrlExceptions
{
    public static ValidationException ShortUrlAlreadyInUse() => new(new[]
    {
        new ValidationFailure(nameof(Contracts.CreateUrlRequest.ShortUrl), "The specified short url is already in use.")
    });
}