using FluentValidation;
using MediatR.Pipeline;

namespace Url.Shortener.Api.Domain;

public class ValidationProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : IValidatableRequestBase
{
    private readonly ILogger<ValidationProcessor<TRequest>> _logger;
    private readonly IValidator<TRequest> _validator;

    public ValidationProcessor(IValidator<TRequest> validator, ILogger<ValidationProcessor<TRequest>> logger)
    {
        _validator = validator;
        _logger = logger;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid) return;

        var validationException = new ValidationException(validationResult.Errors);
        _logger.LogWarning(validationException, "Validation failed for object {@RequestObject}", request);
        throw validationException;
    }
}