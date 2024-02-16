using AutoFixture;
using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidCode
{
    private readonly CreateUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidCode() => _validator = CreateUrlRequestValidatorBuilder.Build();

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenNoExceptionIsThrown(CreateUrlRequest request)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(request));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFails(CreateUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFailsForTheCode(CreateUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(CreateUrlRequest.Code));
    }

    private async Task<ValidationResult> WhenValidatingAsync(CreateUrlRequest request) => await _validator.ValidateAsync(request);

    private class TestData : TheoryData<CreateUrlRequest>
    {
        public TestData()
        {
            var fixture = new Fixture();
            var request = new CreateUrlRequest($"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}");
            Add(request with { Code = string.Empty });
            Add(request with { Code = "   " });
            Add(request with { Code = "tA" });                                                  // string with 2 length
            Add(request with { Code = "tAJqPRr@ywgK5Av*nzu6c@Wg+5#fxWj4Mmnt5o3UuqEqYFsARJT" }); // string with 51 length
            Add(request with { Code = "abcde/ab" });                                            // path separator ('/') character not allowed
            Add(request with { Code = "abcde\\ab" });                                           // invalid relative uri
        }
    }
}