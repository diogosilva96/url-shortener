using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Url.Shortener.Api.Domain;

public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> _logger;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationFailures = new List<ValidationFailure>();

        foreach (var validator in _validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid) continue;
            validationFailures.AddRange(validationResult.Errors);
        }

        if (!validationFailures.Any())
        {
            var validationException = new ValidationException(validationFailures);
            _logger.LogError(validationException, "Validation failed for object of type '{ObjectType}'", typeof(TRequest).FullName);
            throw validationException;
        }

        return await next();
    }
}