using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Redirect;
using Url.Shortener.Api.UnitTests.Domain.Url.Redirect.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Redirect.RedirectUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidShortUrl
{
    private readonly RedirectUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidShortUrl() => _validator = RedirectUrlRequestValidatorBuilder.Build();

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenNoExceptionIsThrown(string? shortUrl)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(shortUrl));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFails(string? shortUrl)
    {
        var validationResult = await WhenValidatingAsync(shortUrl);

        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFailsForTheShortUrl(string? shortUrl)
    {
        var validationResult = await WhenValidatingAsync(shortUrl);

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(RedirectUrlRequest.ShortUrl));
    }

    private async Task<ValidationResult> WhenValidatingAsync(string? shortUrl)
    {
        var request = new RedirectUrlRequest(shortUrl!);

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