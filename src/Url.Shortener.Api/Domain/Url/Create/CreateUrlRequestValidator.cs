using FluentValidation;

namespace Url.Shortener.Api.Domain.Url.Create;

public class CreateUrlRequestValidator : AbstractValidator<CreateUrlRequest>
{
    public CreateUrlRequestValidator()
    {
        RuleFor(x => x.Url).NotEmpty();

        RuleFor(x => x.Url).Custom(EnsureValidUrl())
                           .When(x => !string.IsNullOrWhiteSpace(x.Url));

        RuleFor(x => x.Code!).EnsureValidCode()
                             .When(x => !string.IsNullOrWhiteSpace(x.Code));

        RuleFor(x => x.Code).NotEmpty()
                            .When(x => x.Code is not null);
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