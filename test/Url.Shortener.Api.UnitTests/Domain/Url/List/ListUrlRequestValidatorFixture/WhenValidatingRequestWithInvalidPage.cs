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
    public async Task ThenNoExceptionIsThrown(ListUrlRequest request)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(request));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFails(ListUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFailsForThePage(ListUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(ListUrlRequest.Page));
    }

    private async Task<ValidationResult> WhenValidatingAsync(ListUrlRequest request) => await _validator.ValidateAsync(request);


    private class TestData : TheoryData<ListUrlRequest>
    {
        public TestData()
        {
            var request = new ListUrlRequest(1, 1);
            Add(request with { Page = 0 });
            Add(request with { Page = -1 });
        }
    }
}