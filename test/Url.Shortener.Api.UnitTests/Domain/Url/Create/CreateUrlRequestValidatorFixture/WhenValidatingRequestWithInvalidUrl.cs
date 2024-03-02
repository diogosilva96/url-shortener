using AutoFixture;
using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidUrl
{
    private readonly CreateUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidUrl() => _validator = CreateUrlRequestValidatorBuilder.Build();

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
    public async Task ThenTheValidationFailsForTheUrl(CreateUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(CreateUrlRequest.Url));
    }

    private async Task<ValidationResult> WhenValidatingAsync(CreateUrlRequest request) => await _validator.ValidateAsync(request);


    private class TestData : TheoryData<CreateUrlRequest>
    {
        public TestData()
        {
            var fixture = new Fixture();
            var request = new CreateUrlRequest(string.Empty);
            Add(request with { Url = string.Empty });
            Add(request with { Url = default! });
            Add(request with { Url = "  " });
            Add(request with { Url = fixture.Create<string>() });
            Add(request with { Url = $"http://{fixture.Create<string>()}.com" });
            Add(request with { Url = $"https://{fixture.Create<string>()}" });
        }
    }
}