using AutoFixture;
using FluentValidation;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestHandlerFixture;

public class WhenHandlingRequestAndCodeIsSpecifiedAndCodeIsAlreadyInUse
{
    private readonly CreateUrlRequestHandler _handler;
    private readonly CreateUrlRequest _request;

    public WhenHandlingRequestAndCodeIsSpecifiedAndCodeIsAlreadyInUse()
    {
        var fixture = new Fixture();

        var urlMetadata = new[]
        {
            new UrlMetadataBuilder().Build(),
            new UrlMetadataBuilder().Build()
        };

        var dbContext = new UrlShortenerDbContextBuilder().With(urlMetadata)
                                                          .Build();

        _request = new(fixture.Create<string>(), urlMetadata[0].Code); // short url already exists in the metadata store

        var urlShortener = Substitute.For<ICodeGenerator>();

        _handler = new CreateUrlRequestHandlerBuilder().With(dbContext)
                                                       .With(urlShortener)
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
        var expectedException = CreateUrlExceptions.CodeAlreadyInUse();
        Assert.All(expectedException.Errors, expectedError =>
            Assert.Single(validationException.Errors, error => expectedError.ErrorMessage == error.ErrorMessage &&
                                                               expectedError.PropertyName == error.PropertyName));
    }

    private async Task<string> WhenHandlingAsync() => await _handler.Handle(_request);
}