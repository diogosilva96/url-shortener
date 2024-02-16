using FluentValidation;

namespace Url.Shortener.Api.Domain.Url.Get;

public class GetUrlRequestValidator : AbstractValidator<GetUrlRequest>
{
    public GetUrlRequestValidator()
    {
        RuleFor(x => x.Code).EnsureValidCode();
    }
}