namespace Url.Shortener.Api.Domain.Url.Redirect;

internal record RedirectUrlRequest(string Code) : IValidatableRequest<string>
{ }