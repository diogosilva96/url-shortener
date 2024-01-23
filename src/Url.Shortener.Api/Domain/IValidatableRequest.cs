using MediatR;

namespace Url.Shortener.Api.Domain;

internal interface IValidatableRequest : IRequest, IValidatableRequestBase
{ }

internal interface IValidatableRequest<out TResponse> : IRequest<TResponse>, IValidatableRequestBase
{ }

// marker interface for validation
internal interface IValidatableRequestBase : IBaseRequest
{ }