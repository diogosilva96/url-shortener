using AutoFixture;
using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;
using CreateUrlRequest = Url.Shortener.Api.Contracts.CreateUrlRequest;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CreateUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidUrl
{
    private readonly CreateUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidUrl() => _validator = CreateUrlRequestValidatorBuilder.Build();

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenNoExceptionIsThrown(string? url)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(url));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFails(string? url)
    {
        var validationResult = await WhenValidatingAsync(url);

        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFailsForTheUrl(string? url)
    {
        var validationResult = await WhenValidatingAsync(url);

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(CreateUrlRequest.Url));
    }

    private async Task<ValidationResult> WhenValidatingAsync(string? url)
    {
        var request = new Api.Domain.Url.Create.CreateUrlRequest(url!);

        return await _validator.ValidateAsync(request);
    }

    private class TestData : TheoryData<string?>
    {
        public TestData()
        {
            var fixture = new Fixture();
            Add(string.Empty);
            Add(default!);
            Add("  ");
            Add(fixture.Create<string>());
            Add($"http://{fixture.Create<string>()}.com");
            Add($"https://{fixture.Create<string>()}");
        }
    }
}