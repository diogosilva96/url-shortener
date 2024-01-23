using FluentValidation;

namespace Url.Shortener.Api.Domain.Url.Get;

internal class GetUrlRequestValidator : AbstractValidator<GetUrlRequest>
{
    public GetUrlRequestValidator()
    {
        // TODO: make an extension method for this so that it can be reused with the CreateUrl endpoint
        RuleFor(x => x.ShortUrl).MinimumLength(5)
                                .MaximumLength(50)
                                .Must(x => Uri.IsWellFormedUriString(x, UriKind.Relative))
                                .WithMessage("The '{PropertyName}' must be a valid relative uri.")
                                .Must(x => !x.Contains('/'))
                                .WithMessage("The '{PropertyName}' must not contain a path separator character ('/').");
    }
}