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
    public async Task ThenNoExceptionIsThrown(CreateUrlRequest request)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(request));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationSucceeds(CreateUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.True(validationResult.IsValid);
    }

    private async Task<ValidationResult> WhenValidatingAsync(CreateUrlRequest request) => await _validator.ValidateAsync(request);


    private class TestData : TheoryData<CreateUrlRequest>
    {
        public TestData()
        {
            var fixture = new Fixture();
            var validUrl = $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}";
            var request = new CreateUrlRequest(validUrl);
            Add(request);
            Add(request with { Code = "abc" });                                                // 3 length string
            Add(request with { Code = "tAJqPRr@ywgK5Av+nzu6c@WgB5AfxWj4Mmnt5o3UuqEqYFsARJ" }); // 50 length string
        }
    }
}