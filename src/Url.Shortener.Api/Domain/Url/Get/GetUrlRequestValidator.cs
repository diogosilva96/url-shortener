using FluentValidation;

namespace Url.Shortener.Api.Domain.Url.Get;

internal class GetUrlRequestValidator : AbstractValidator<GetUrlRequest>
{
    public GetUrlRequestValidator()
    {
        RuleFor(x => x.ShortUrl).NotEmpty();

        RuleFor(x => x.ShortUrl).MaximumLength(50);

        RuleFor(x => x.ShortUrl).Must(url => Uri.TryCreate(url, UriKind.Relative, out _))
                                .WithMessage("The '{PropertyName}' is not a valid relative uri.");
    }
}