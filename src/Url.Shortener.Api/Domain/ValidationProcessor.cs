using FluentValidation;
using FluentValidation.Results;
using MediatR.Pipeline;

namespace Url.Shortener.Api.Domain;

internal class ValidationProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationProcessor<TRequest>> _logger;
    
    public ValidationProcessor(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationProcessor<TRequest>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var validationFailures = new List<ValidationFailure>();

        foreach (var validator in _validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid) continue;
            validationFailures.AddRange(validationResult.Errors);
        }

        if (!validationFailures.Any()) return;
        
        var validationException = new ValidationException(validationFailures);
        _logger.LogError(validationException, "Validation failed for object of type '{ObjectType}'", typeof(TRequest).FullName);
        throw validationException;
    }
}