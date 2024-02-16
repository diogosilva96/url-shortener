using FluentValidation;
using Url.Shortener.Api.Contracts;

namespace Url.Shortener.Api.Domain;

internal static class AbstractValidatorExtensions
{
    public static void ApplyPagedRequestRules<T>(this AbstractValidator<T> validator) where T : IPagedRequest
    {
        validator.RuleFor(x => x.PageSize)
                 .GreaterThan(0)
                 .LessThanOrEqualTo(200);

        validator.RuleFor(x => x.Page)
                 .GreaterThan(0);
    }
}