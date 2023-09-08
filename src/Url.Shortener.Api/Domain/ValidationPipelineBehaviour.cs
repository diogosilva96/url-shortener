using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Url.Shortener.Api.Domain;

public class ValidationPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

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
            throw new ValidationException(validationFailures);
        }

        return await next();
    }
}