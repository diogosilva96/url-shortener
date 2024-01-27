using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Get;
using Url.Shortener.Api.UnitTests.Domain.Url.Get.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Get.GetUrlRequestValidatorFixture;

public class WhenValidatingRequest
{
    private readonly GetUrlRequestValidator _validator;
    
    public WhenValidatingRequest() => _validator = GetUrlRequestValidatorBuilder.Build();

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenNoExceptionIsThrown(string code)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(code));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationSucceeds(string code)
    {
        var validationResult = await WhenValidatingAsync(code);

        Assert.True(validationResult.IsValid);
    }

    private async Task<ValidationResult> WhenValidatingAsync(string code)
    {
        var request = new GetUrlRequest(code);
        return await _validator.ValidateAsync(request);
    }

    private class TestData : TheoryData<string>
    {
        public TestData()
        {
            Add("abcde");                                              // 5 length string
            Add("tAJqPRr@ywgK5Av+nzu6c@WgB5AfxWj4Mmnt5o3UuqEqYFsARJ"); // 50 length string
        }
    }
}