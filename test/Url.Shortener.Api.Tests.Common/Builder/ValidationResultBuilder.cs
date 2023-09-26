using FluentValidation.Results;

namespace Url.Shortener.Api.Tests.Common.Builder;

public class ValidationResultBuilder
{
    private readonly List<ValidationFailure> _failures;

    public ValidationResultBuilder() => _failures = new();

    public ValidationResult Build() => new(_failures);

    public ValidationResultBuilder WithFailures(IEnumerable<ValidationFailure> failures)
    {
        _failures.AddRange(failures);

        return this;
    }
}