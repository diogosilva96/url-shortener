using MediatR;

namespace Url.Shortener.Api.Domain;

public interface IValidatableRequest : IRequest, IValidatableRequestBase
{ }

public interface IValidatableRequest<out TResponse> : IRequest<TResponse>, IValidatableRequestBase
{ }

// marker interface for validation
public interface IValidatableRequestBase : IBaseRequest
{ }