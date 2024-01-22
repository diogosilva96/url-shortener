using AutoFixture;
using Url.Shortener.Api.Domain.Url.Redirect;
using Url.Shortener.Api.Exceptions;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Redirect.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Redirect.RedirectUrlRequestHandlerFixture;

public class WhenHandlingRequestAndUrlMetadataCannotBeFound
{
    private readonly RedirectUrlRequestHandler _handler;
    private readonly RedirectUrlRequest _request;

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

        _handler = new RedirectUrlRequestHandlerBuilder().With(dbContext)
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

    [Fact]
    public async Task ThenTheExpectedNotFoundExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        var notFoundException = (exception as NotFoundException)!;
        var expectedException = RedirectUrlExceptions.UrlNotFound();

        Assert.Equal(expectedException.Message, notFoundException.Message);
    }

    private async Task<string> WhenHandlingAsync() => await _handler.Handle(_request);
}