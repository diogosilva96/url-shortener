using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.Exceptions;
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
        _request = new("123");
        
        var metadata = new[]
        {
            new UrlMetadataBuilder().With(x => x.Code = "1")
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.Code = "2")
                                    .Build()
        };
        

        var dbContext = new UrlShortenerDbContextBuilder().With(metadata)
                                                           .Build();

        _handler = new GetUrlRequestHandlerBuilder().With(dbContext)
                                                    .Build();
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
        var expectedException = GetUrlExceptions.UrlNotFound();

        Assert.Equal(expectedException.Message, notFoundException.Message);
    }

    private async Task<Contracts.UrlMetadata> WhenHandlingAsync() => await _handler.Handle(_request, CancellationToken.None);
}