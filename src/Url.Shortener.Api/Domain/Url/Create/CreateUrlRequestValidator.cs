using FluentValidation;

namespace Url.Shortener.Api.Domain.Url.Create;

internal class CreateUrlRequestValidator : AbstractValidator<CreateUrlRequest>
{
    public CreateUrlRequestValidator()
    {
        RuleFor(x => x.Url).NotEmpty();

        RuleFor(x => x.Url).Custom(EnsureValidUrl())
                           .When(x => !string.IsNullOrWhiteSpace(x.Url));

        RuleFor(x => x.ShortUrl).MinimumLength(5)
                                .MaximumLength(50)
                                .Must(x => Uri.IsWellFormedUriString(x, UriKind.Relative))
                                .WithMessage("The '{PropertyName}' must be a valid relative uri.")
                                .Must(x => !x.Contains('/'))
                                .WithMessage("The '{PropertyName}' must not contain a path separator character ('/').")
                                .When(x => !string.IsNullOrWhiteSpace(x.ShortUrl));

        RuleFor(x => x.ShortUrl).NotEmpty()
                                .When(x => x.ShortUrl is not null);
    }

    private static Action<string, ValidationContext<CreateUrlRequest>> EnsureValidUrl() =>
        (url, context) =>
        {
            context.MessageFormatter.AppendPropertyName(context.PropertyPath);

            if (!Uri.TryCreate(url, UriKind.Absolute, out var parsedUri))
            {
                context.AddFailure(context.MessageFormatter.BuildMessage("The '{PropertyName}' is not a valid absolute url."));
                return;
            }

            // ReSharper disable once InvertIf
            if (parsedUri.Scheme != Uri.UriSchemeHttps)
            {
                context.AddFailure(context.MessageFormatter.BuildMessage("The '{PropertyName}' must be secure."));
                return;
            }

            if (!parsedUri.Authority.Contains('.'))
            {
                context.AddFailure(context.MessageFormatter.BuildMessage("The '{PropertyName}' must have a valid authority."));
            }
        };
}