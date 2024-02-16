using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.List;
using Url.Shortener.Api.UnitTests.Domain.Url.List.Builder;
using Xunit;
using ListUrlRequest = Url.Shortener.Api.Domain.Url.List.ListUrlRequest;

namespace Url.Shortener.Api.UnitTests.Domain.Url.List.ListUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidPage
{
    private readonly ListUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidPage() => _validator = ListUrlRequestValidatorBuilder.Build();

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenNoExceptionIsThrown(int page)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(page));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFails(int page)
    {
        var validationResult = await WhenValidatingAsync(page);

        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFailsForThePage(int page)
    {
        var validationResult = await WhenValidatingAsync(page);

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(ListUrlRequest.Page));
    }

    private async Task<ValidationResult> WhenValidatingAsync(int page)
    {
        var request = new ListUrlRequest(1, page);
        return await _validator.ValidateAsync(request);
    }

    private class TestData : TheoryData<int>
    {
        public TestData()
        {
            Add(0);
            Add(-1);
        }
    }
}