using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.UnitTests.Domain.Url.Get.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Get.GetUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidShortUrl
{
    private readonly GetUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidShortUrl() => _validator = GetUrlRequestValidatorBuilder.Build();

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

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(GetUrlRequest.ShortUrl));
    }

    private async Task<ValidationResult> WhenValidatingAsync(string? shortUrl)
    {
        var request = new GetUrlRequest(shortUrl!);

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