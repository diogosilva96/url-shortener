using AutoFixture;
using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestValidatorFixture;

public class WhenValidatingRequest
{
    private readonly CreateUrlRequestValidator _validator;
    
    public WhenValidatingRequest() => _validator = CreateUrlRequestValidatorBuilder.Build();

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenNoExceptionIsThrown(string url, string? shortUrl)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(url, shortUrl));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationSucceeds(string url, string? shortUrl)
    {
        var validationResult = await WhenValidatingAsync(url, shortUrl);

        Assert.True(validationResult.IsValid);
    }

    private async Task<ValidationResult> WhenValidatingAsync(string url, string? shortUrl)
    {
        var request = new CreateUrlRequest(url, shortUrl);
        return await _validator.ValidateAsync(request);
    }

    private class TestData : TheoryData<string, string?>
    {
        public TestData()
        {
            var fixture = new Fixture();
            var validUrl = $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}";
            Add(validUrl, default);
            Add(validUrl, "abcde");                                              // 5 length string
            Add(validUrl, "tAJqPRr@ywgK5Av*nzu6c@Wg+5#fxWj4Mmnt5o3UuqEqYFsARJ"); // 50 length string
        }
    }
}