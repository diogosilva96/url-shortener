using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Url.Shortener.Api.Domain;

namespace Url.Shortener.Api.UnitTests.Domain.Builder;

public class ValidationProcessorBuilder<T> where T : IValidatableRequestBase
{
    private ILogger<ValidationProcessor<T>> _logger;
    private IValidator<T> _validator;

    public ValidationProcessorBuilder()
    {
        _validator = Substitute.For<IValidator<T>>();
        _logger = Substitute.For<ILogger<ValidationProcessor<T>>>();
    }

    public ValidationProcessor<T> Build() => new(_validator, _logger);

    public ValidationProcessorBuilder<T> With(IValidator<T> validator)
    {
        _validator = validator;

        return this;
    }

    public ValidationProcessorBuilder<T> With(ILogger<ValidationProcessor<T>> logger)
    {
        _logger = logger;

        return this;
    }
}