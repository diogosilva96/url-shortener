using Url.Shortener.Api.Domain.Url.Redirect;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Redirect.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Redirect.RedirectUrlRequestHandlerFixture;

public class WhenHandlingRequest
{
    private readonly string _expectedUrl;
    private readonly RedirectUrlRequestHandler _handler;
    private readonly RedirectUrlRequest _request;

    public WhenHandlingRequest()
    {
        var expectedUrlMetadata = new UrlMetadataBuilder().Build();

        var urlMetadata = new[]
        {
            expectedUrlMetadata,
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build()
        };

        var dbContext = new UrlShortenerDbContextBuilder().With(urlMetadata)
                                                          .Build();

        _handler = new RedirectUrlRequestHandlerBuilder().With(dbContext)
                                                         .Build();

        _request = new(expectedUrlMetadata.Code);

        _expectedUrl = expectedUrlMetadata.FullUrl;
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenAUrlIsReturned()
    {
        var url = await WhenHandlingAsync();

        Assert.NotEmpty(url);
    }

    [Fact]
    public async Task ThenTheExpectedUrlIsReturned()
    {
        var url = await WhenHandlingAsync();

        Assert.Equal(_expectedUrl, url);
    }

    private async Task<string> WhenHandlingAsync() => await _handler.Handle(_request);
}