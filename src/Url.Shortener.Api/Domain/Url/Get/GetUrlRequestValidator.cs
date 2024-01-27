using FluentValidation;

namespace Url.Shortener.Api.Domain.Url.Get;

internal class GetUrlRequestValidator : AbstractValidator<GetUrlRequest>
{
    public GetUrlRequestValidator()
    {
        RuleFor(x => x.Code).EnsureValidCode();
    }
}