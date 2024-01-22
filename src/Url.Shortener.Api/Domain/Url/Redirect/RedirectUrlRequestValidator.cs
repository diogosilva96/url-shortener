using FluentValidation;

namespace Url.Shortener.Api.Domain.Url.Redirect;

internal class RedirectUrlRequestValidator : AbstractValidator<RedirectUrlRequest>
{
    public RedirectUrlRequestValidator()
    {
        RuleFor(x => x.ShortUrl).NotEmpty();

        RuleFor(x => x.ShortUrl).MaximumLength(50);

        RuleFor(x => x.ShortUrl).Must(url => Uri.TryCreate(url, UriKind.Relative, out _))
                                .WithMessage("The '{PropertyName}' is not a valid relative uri.");
    }
}