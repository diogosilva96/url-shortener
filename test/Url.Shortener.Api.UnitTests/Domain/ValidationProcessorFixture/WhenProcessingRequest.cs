using AutoFixture;
using FluentValidation;
using NSubstitute;
using Url.Shortener.Api.Domain;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Tests.Common.Builder;
using Url.Shortener.Api.UnitTests.Domain.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.ValidationProcessorFixture;

public class WhenProcessingRequest
{
    // we can use any request that implements 'IValidatableRequest' - in this case we use 'CreateUrlRequest'
    private readonly ValidationProcessor<CreateUrlRequest> _processor;
    private readonly CreateUrlRequest _request;
    private readonly IValidator<CreateUrlRequest> _validator;

    public WhenProcessingRequest()
    {
        var fixture = new Fixture();
        _request = new(fixture.Create<string>());

        _validator = Substitute.For<IValidator<CreateUrlRequest>>();
        _validator.ValidateAsync(Arg.Any<CreateUrlRequest>(), Arg.Any<CancellationToken>())
                  .Returns(new ValidationResultBuilder().Build());

        _processor = new ValidationProcessorBuilder<CreateUrlRequest>().With(_validator)
                                                                       .Build();
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenProcessingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenARequestIsValidated()
    {
        await WhenProcessingAsync();

        await _validator.ReceivedWithAnyArgs(1)
                        .ValidateAsync(Arg.Any<CreateUrlRequest>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ThenTheExpectedRequestIsValidated()
    {
        await WhenProcessingAsync();

        await _validator.Received(1)
                        .ValidateAsync(_request, Arg.Any<CancellationToken>());
    }

    private async Task WhenProcessingAsync() => await _processor.Process(_request, CancellationToken.None);
}