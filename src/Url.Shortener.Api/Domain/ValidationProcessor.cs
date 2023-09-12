using FluentValidation;
using MediatR.Pipeline;

namespace Url.Shortener.Api.Domain;

internal class ValidationProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : IValidatableRequest
{
    private readonly IValidator<TRequest> _validator;
    private readonly ILogger<ValidationProcessor<TRequest>> _logger;
    
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
        _logger.LogWarning(validationException, "Validation failed for object of type '{ObjectType}'", typeof(TRequest).FullName);
        throw validationException;
    }
}