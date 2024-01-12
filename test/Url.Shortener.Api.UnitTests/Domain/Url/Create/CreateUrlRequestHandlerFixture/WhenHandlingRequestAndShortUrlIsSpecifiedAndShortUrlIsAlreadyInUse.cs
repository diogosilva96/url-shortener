using AutoFixture;
using FluentValidation;
using Microsoft.Extensions.Internal;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestHandlerFixture;

public class WhenHandlingRequestAndShortUrlIsSpecifiedAndShortUrlIsAlreadyInUse
{
    private readonly CreateUrlRequestHandler _handler;
    private readonly CreateUrlRequest _request;

    public WhenHandlingRequestAndShortUrlIsSpecifiedAndShortUrlIsAlreadyInUse()
    {
        var fixture = new Fixture();

        var urlMetadata = new[]
        {
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build()
        };

        var dbContext = new UrlShortenerDbContextBuilder().With(urlMetadata)
                                                          .Build();

        _request = new(fixture.Create<string>(), urlMetadata[0].ShortUrl); // short url already exists in the metadata store

        var urlShortener = Substitute.For<IUrlShortener>();

        var expectedDateTime = fixture.Create<DateTimeOffset>();
        var clock = Substitute.For<ISystemClock>();
        clock.UtcNow.Returns(expectedDateTime);

        _handler = new CreateUrlRequestHandlerBuilder().With(dbContext)
                                                       .With(urlShortener)
                                                       .With(clock)
                                                       .Build();
    }

    [Fact]
    public async Task ThenAnExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task ThenAValidationExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        Assert.IsType<ValidationException>(exception);
    }

    [Fact]
    public async Task ThenTheExpectedValidationExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenHandlingAsync);

        var validationException = (exception as ValidationException)!;
        var expectedException = CreateUrlExceptions.ShortUrlAlreadyInUse();
        Assert.All(expectedException.Errors, expectedError =>
            Assert.Single(validationException.Errors, error => expectedError.ErrorMessage == error.ErrorMessage &&
                                                               expectedError.PropertyName == error.PropertyName));
    }

    private async Task<string> WhenHandlingAsync() => await _handler.Handle(_request);
}