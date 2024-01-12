using AutoFixture;
using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;
using CreateUrlRequest = Url.Shortener.Api.Contracts.CreateUrlRequest;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidShortUrl
{
    private readonly CreateUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidShortUrl() => _validator = CreateUrlRequestValidatorBuilder.Build();

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

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(CreateUrlRequest.ShortUrl));
    }

    private async Task<ValidationResult> WhenValidatingAsync(string? shortUrl)
    {
        var fixture = new Fixture();
        var request = new Api.Domain.Url.Create.CreateUrlRequest($"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}", shortUrl!);

        return await _validator.ValidateAsync(request);
    }

    private class TestData : TheoryData<string?>
    {
        public TestData()
        {
            Add(string.Empty);
            Add("   ");
            Add("tAJq");                                                // string with 4 length
            Add("tAJqPRr@ywgK5Av*nzu6c@Wg+5#fxWj4Mmnt5o3UuqEqYFsARJT"); // string with 51 length
            Add("abcde/ab");                                            // path separator ('/') character not allowed
            Add("abcde\\ab");                                           // invalid relative uri
        }
    }
}