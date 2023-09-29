using AutoFixture;
using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestValidatorFixture;

public class WhenValidatingRequest
{
    private readonly CreateUrlRequest _request;
    private readonly CreateUrlRequestValidator _validator;

    public WhenValidatingRequest()
    {
        var fixture = new Fixture();
        _request = new($"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}");

        _validator = CreateUrlRequestValidatorBuilder.Build();
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