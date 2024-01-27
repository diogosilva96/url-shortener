using AutoFixture;
using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;
using CreateUrlRequest = Url.Shortener.Api.Contracts.CreateUrlRequest;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidCode
{
    private readonly CreateUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidCode() => _validator = CreateUrlRequestValidatorBuilder.Build();

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

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(CreateUrlRequest.Code));
    }

    private async Task<ValidationResult> WhenValidatingAsync(string? code)
    {
        var fixture = new Fixture();
        var request = new Api.Domain.Url.Create.CreateUrlRequest($"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}", code!);

        return await _validator.ValidateAsync(request);
    }

    private class TestData : TheoryData<string?>
    {
        public TestData()
        {
            Add(string.Empty);
            Add("   ");
            Add("tA");                                                // string with 2 length
            Add("tAJqPRr@ywgK5Av*nzu6c@Wg+5#fxWj4Mmnt5o3UuqEqYFsARJT"); // string with 51 length
            Add("abcde/ab");                                            // path separator ('/') character not allowed
            Add("abcde\\ab");                                           // invalid relative uri
        }
    }
}