using FluentValidation;
using FluentValidation.Results;

namespace Url.Shortener.Api.Domain.Url.Create;

public static class CreateUrlExceptions
{
    public static ValidationException CodeAlreadyInUse() => new(new[]
    {
        new ValidationFailure(nameof(Contracts.CreateUrlRequest.Code), "The specified code is already in use.")
    });
}