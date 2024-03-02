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
    public async Task ThenNoExceptionIsThrown(RedirectUrlRequest request)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(request));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFails(RedirectUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFailsForTheCode(RedirectUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(RedirectUrlRequest.Code));
    }

    private async Task<ValidationResult> WhenValidatingAsync(RedirectUrlRequest request) => await _validator.ValidateAsync(request);

    private class TestData : TheoryData<RedirectUrlRequest>
    {
        public TestData()
        {
            var request = new RedirectUrlRequest(string.Empty);
            Add(request with { Code = string.Empty });
            Add(request with { Code = default! });
            Add(request with { Code = "  " });
            Add(request with { Code = "EpyD2QL2ecThCbgX1flUmiHXtEpyD2QL2ecThCbgX1flUmiHXtA" }); // more than 50 characters long
        }
    }
}