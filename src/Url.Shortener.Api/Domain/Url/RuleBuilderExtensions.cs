using FluentValidation;

namespace Url.Shortener.Api.Domain.Url;

internal static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> EnsureValidShortUrl<T>(this IRuleBuilder<T, string> ruleBuilder) => 
        ruleBuilder.MinimumLength(5)
                   .MaximumLength(50)
                   .Must(x => Uri.IsWellFormedUriString(x, UriKind.Relative))
                   .WithMessage("The '{PropertyName}' must be a valid relative uri.")
                   .Must(x => !x.Contains('/'))
                   .WithMessage("The '{PropertyName}' must not contain a path separator character ('/').");
}