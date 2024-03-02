using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.UnitTests.Domain.Url.Get.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Get.GetUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidCode
{
    private readonly GetUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidCode() => _validator = GetUrlRequestValidatorBuilder.Build();

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenNoExceptionIsThrown(GetUrlRequest request)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(request));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFails(GetUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFailsForTheCode(GetUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(GetUrlRequest.Code));
    }

    private async Task<ValidationResult> WhenValidatingAsync(GetUrlRequest request) => await _validator.ValidateAsync(request);

    private class TestData : TheoryData<GetUrlRequest>
    {
        public TestData()
        {
            var request = new GetUrlRequest("abc");
            Add(request with { Code = string.Empty });
            Add(request with { Code = "   " });
            Add(request with { Code = "tA" });                                                  // string with 2 length
            Add(request with { Code = "tAJqPRr@ywgK5Av*nzu6c@Wg+5#fxWj4Mmnt5o3UuqEqYFsARJT" }); // string with 51 length
            Add(request with { Code = "abcde/ab" });                                            // path separator ('/') character not allowed
            Add(request with { Code = "abcde\\ab" });                                           // invalid relative uri
        }
    }
}