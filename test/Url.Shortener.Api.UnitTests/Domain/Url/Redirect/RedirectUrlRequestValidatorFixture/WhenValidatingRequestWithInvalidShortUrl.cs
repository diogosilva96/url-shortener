using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Redirect;
using Url.Shortener.Api.UnitTests.Domain.Url.Redirect.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Redirect.RedirectUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidCode
{
    private readonly RedirectUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidCode() => _validator = RedirectUrlRequestValidatorBuilder.Build();

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenNoExceptionIsThrown(string? code)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(code));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFails(string? code)
    {
        var validationResult = await WhenValidatingAsync(code);

        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFailsForTheCode(string? code)
    {
        var validationResult = await WhenValidatingAsync(code);

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(RedirectUrlRequest.Code));
    }

    private async Task<ValidationResult> WhenValidatingAsync(string? code)
    {
        var request = new RedirectUrlRequest(code!);

        return await _validator.ValidateAsync(request);
    }

    private class TestData : TheoryData<string?>
    {
        public TestData()
        {
            Add(string.Empty);
            Add(default!);
            Add("  ");
            Add("EpyD2QL2ecThCbgX1flUmiHXtEpyD2QL2ecThCbgX1flUmiHXtA"); // more than 50 characters long
        }
    }
}