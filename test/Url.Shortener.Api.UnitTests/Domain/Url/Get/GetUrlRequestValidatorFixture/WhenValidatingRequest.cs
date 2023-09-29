using AutoFixture;
using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.UnitTests.Domain.Url.Get.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Get.GetUrlRequestValidatorFixture;

public class WhenValidatingRequest
{
    private readonly GetUrlRequest _request;
    private readonly GetUrlRequestValidator _validator;

    public WhenValidatingRequest()
    {
        var fixture = new Fixture();
        _request = new(fixture.Create<string>());

        _validator = GetUrlRequestValidatorBuilder.Build();
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenValidatingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenTheValidationSucceeds()
    {
        var validationResult = await WhenValidatingAsync();

        Assert.True(validationResult.IsValid);
    }

    private async Task<ValidationResult> WhenValidatingAsync() => await _validator.ValidateAsync(_request);
}