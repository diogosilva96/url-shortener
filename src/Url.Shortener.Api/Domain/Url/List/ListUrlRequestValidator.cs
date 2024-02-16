using FluentValidation;

namespace Url.Shortener.Api.Domain.Url.List;

internal class ListUrlRequestValidator : AbstractValidator<ListUrlRequest>
{
    public ListUrlRequestValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);

        RuleFor(x => x.PageSize).GreaterThan(0)
                                .LessThanOrEqualTo(200);
    }
}