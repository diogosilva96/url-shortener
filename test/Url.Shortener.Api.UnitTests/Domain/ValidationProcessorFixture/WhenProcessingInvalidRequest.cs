using AutoFixture;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using Url.Shortener.Api.Domain;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Tests.Common.Builder;
using Url.Shortener.Api.UnitTests.Domain.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.ValidationProcessorFixture;

public class WhenProcessingInvalidRequest
{
    // we can use any request that implements 'IValidatableRequest' - in this case we use 'CreateUrlRequest'
    private readonly ValidationProcessor<CreateUrlRequest> _processor;
    private readonly CreateUrlRequest _request;
    private readonly ValidationFailure[] _validationFailures;

    public WhenProcessingInvalidRequest()
    {
        var fixture = new Fixture();
        _request = new(fixture.Create<string>());

        _validationFailures = fixture.CreateMany<ValidationFailure>(5).ToArray();
        var validator = Substitute.For<IValidator<CreateUrlRequest>>();
        validator.ValidateAsync(Arg.Any<CreateUrlRequest>(), Arg.Any<CancellationToken>())
                 .Returns(new ValidationResultBuilder().WithFailures(_validationFailures).Build());

        _processor = new ValidationProcessorBuilder<CreateUrlRequest>().With(validator)
                                                                       .Build();
    }

    [Fact]
    public async Task ThenAnExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenProcessingAsync);

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task ThenAValidationExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenProcessingAsync);

        Assert.IsType<ValidationException>(exception);
    }

    [Fact]
    public async Task ThenAValidationExceptionIsThrownWithTheExpectedValidationFailures()
    {
        var exception = await Record.ExceptionAsync(WhenProcessingAsync);

        var validationException = (exception as ValidationException)!;
        Assert.Multiple
        (
            () => Assert.Equal(_validationFailures.Length, validationException.Errors.Count()),
            () => Assert.All(_validationFailures,
                expectedFailure => Assert.Contains(validationException.Errors, failure => failure == expectedFailure))
        );
    }

    private async Task WhenProcessingAsync() => await _processor.Process(_request, CancellationToken.None);
}