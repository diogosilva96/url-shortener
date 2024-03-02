namespace Url.Shortener.Api.Domain.Url.Redirect;

public record RedirectUrlRequest(string Code) : IValidatableRequest<string>
{ }