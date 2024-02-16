using FluentValidation;
using Url.Shortener.Api.Contracts;

namespace Url.Shortener.Api.Domain.Url.List;

internal class ListUrlRequestValidator : AbstractValidator<ListUrlRequest>
{
    public ListUrlRequestValidator()
    {
        this.ApplyPagedRequestRules();
    }
}