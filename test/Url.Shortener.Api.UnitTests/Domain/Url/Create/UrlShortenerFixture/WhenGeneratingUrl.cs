using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Tests.Common.Domain.Url.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.UrlShortenerFixture;

public class WhenGeneratingUrl
{
    private readonly UrlShortenerOptions _options;
    private readonly UrlShortener _urlShortener;

    public WhenGeneratingUrl()
    {
        _options = new UrlShortenerOptionsBuilder().With(x => x.UrlSize = 10)
                                                   .With(x => x.Characters = "_-ABCabc_-")
                                                   .Build();

        _urlShortener = new UrlShortenerBuilder().With(_options)
                                                 .Build();
    }

    [Fact]
    public void ThenNoExceptionIsThrown()
    {
        var exception = Record.Exception(WhenGenerating);

        Assert.Null(exception);
    }

    [Fact]
    public void ThenAUrlIsReturned()
    {
        var shortUrl = WhenGenerating();

        Assert.NotEmpty(shortUrl);
    }

    [Fact]
    public void ThenAUrlIsReturnedWithTheExpectedSize()
    {
        var shortUrl = WhenGenerating();

        Assert.Equal(_options.UrlSize, shortUrl.Length);
    }

    [Fact]
    public void ThenAUrlIsReturnedWithinTheExpectedCharactersRange()
    {
        var shortUrl = WhenGenerating();

        Assert.All(shortUrl, character => Assert.Contains(character, _options.Characters));
    }

    private string WhenGenerating() => _urlShortener.GenerateUrl();
}