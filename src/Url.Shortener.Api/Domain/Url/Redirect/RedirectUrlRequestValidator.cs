using FluentValidation;

namespace Url.Shortener.Api.Domain.Url.Redirect;

internal class RedirectUrlRequestValidator : AbstractValidator<RedirectUrlRequest>
{
    public RedirectUrlRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();

        RuleFor(x => x.Code).MaximumLength(50);

        RuleFor(x => x.Code).Must(url => Uri.TryCreate(url, UriKind.Relative, out _))
                            .WithMessage("The '{PropertyName}' is not a valid relative uri.");
    }
}