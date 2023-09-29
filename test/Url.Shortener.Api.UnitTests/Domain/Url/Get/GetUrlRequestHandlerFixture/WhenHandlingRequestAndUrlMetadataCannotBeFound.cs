using AutoFixture;
using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.Exceptions;
using Url.Shortener.Api.UnitTests.Data;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Get.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Get.GetUrlRequestHandlerFixture;

public class WhenHandlingRequestAndUrlMetadataCannotBeFound
{
    private readonly GetUrlRequestHandler _handler;
    private readonly GetUrlRequest _request;

    public WhenHandlingRequestAndUrlMetadataCannotBeFound()
    {
        var fixture = new Fixture();

        var urlMetadata = new[]
        {
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build()
        };

        var dbContext = new UrlShortenerDbContextBuilder().With(urlMetadata)
                                                          .Build();

        _handler = new GetUrlRequestHandlerBuilder().With(dbContext)
                                                    .Build();

        _request = new(fixture.Create<string>());
    }

    [Fact]
    public async Task ThenAnExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task ThenANotFoundExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.IsType<NotFoundException>(exception);
    }

    private async Task<string> WhenHandlingAsync() => await _handler.Handle(_request);
}