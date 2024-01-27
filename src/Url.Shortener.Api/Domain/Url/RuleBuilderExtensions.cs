using FluentValidation;
using Url.Shortener.Data.Configuration;

namespace Url.Shortener.Api.Domain.Url;

internal static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> EnsureValidCode<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.MinimumLength(5)
                   .MaximumLength(Constants.MaxCodeLength)
                   .Must(x => Uri.IsWellFormedUriString(x, UriKind.Relative))
                   .WithMessage("The '{PropertyName}' must be a valid relative uri.")
                   .Must(x => !x.Contains('/'))
                   .WithMessage("The '{PropertyName}' must not contain a path separator character ('/').");
}