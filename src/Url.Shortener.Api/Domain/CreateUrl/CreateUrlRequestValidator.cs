using FluentValidation;

namespace Url.Shortener.Api.Domain.CreateUrl;

internal class CreateUrlRequestValidator : AbstractValidator<CreateUrlRequest>
{
    public CreateUrlRequestValidator()
    {
        RuleFor(x => x.FullUrl).NotEmpty();
        
        RuleFor(x => x.FullUrl).Custom(EnsureValidFullUrl())
                               .When(x => !string.IsNullOrWhiteSpace(x.FullUrl));
    }

    private static Action<string, ValidationContext<CreateUrlRequest>> EnsureValidFullUrl() => 
        (url, context) =>
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var parsedUri))
            {
                context.AddFailure("The '{PropertyName}' is not a valid url.");
                return;
            }
                                   
            // ReSharper disable once InvertIf
            if (parsedUri.Scheme != Uri.UriSchemeHttps)
            {
                context.AddFailure("The '{PropertyName}' must be an secure url.");
                return;
            }
        };
}