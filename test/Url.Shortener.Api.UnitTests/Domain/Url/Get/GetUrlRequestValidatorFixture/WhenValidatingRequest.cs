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
    public async Task ThenNoExceptionIsThrown(GetUrlRequest request)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(request));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationSucceeds(GetUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.True(validationResult.IsValid);
    }

    private async Task<ValidationResult> WhenValidatingAsync(GetUrlRequest request) => await _validator.ValidateAsync(request);

    private class TestData : TheoryData<GetUrlRequest>
    {
        public TestData()
        {
            Add(new("abc"));                                                // 3 length string
            Add(new("tAJqPRr@ywgK5Av+nzu6c@WgB5AfxWj4Mmnt5o3UuqEqYFsARJ")); // 50 length string
        }
    }
}