using Microsoft.Extensions.Options;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Tests.Common.Domain.Url.Builder;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;

internal class UrlShortenerBuilder
{
    private readonly IOptions<UrlShortenerOptions> _options;

    public UrlShortenerBuilder()
    {
        _options = Substitute.For<IOptions<UrlShortenerOptions>>();
        AssignOptions(new UrlShortenerOptionsBuilder().Build());
    }

    public UrlShortener Build() => new(_options);

    public UrlShortenerBuilder With(UrlShortenerOptions options)
    {
        AssignOptions(options);

        return this;
    }

    private void AssignOptions(UrlShortenerOptions options) => _options.Value.Returns(options);
}