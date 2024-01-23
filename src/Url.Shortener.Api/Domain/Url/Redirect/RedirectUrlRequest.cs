namespace Url.Shortener.Api.Domain.Url.Redirect;

internal record RedirectUrlRequest(string ShortUrl) : IValidatableRequest<string>
{ }