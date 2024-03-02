using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.List;
using Url.Shortener.Api.UnitTests.Domain.Url.List.Builder;
using Xunit;
using ListUrlRequest = Url.Shortener.Api.Domain.Url.List.ListUrlRequest;

namespace Url.Shortener.Api.UnitTests.Domain.Url.List.ListUrlRequestValidatorFixture;

public class WhenValidatingRequest
{
    private readonly ListUrlRequestValidator _validator;

    public WhenValidatingRequest() => _validator = ListUrlRequestValidatorBuilder.Build();

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenNoExceptionIsThrown(ListUrlRequest request)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(request));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationSucceeds(ListUrlRequest request)
    {
        var validationResult = await WhenValidatingAsync(request);

        Assert.True(validationResult.IsValid);
    }

    private async Task<ValidationResult> WhenValidatingAsync(ListUrlRequest request) => await _validator.ValidateAsync(request);


    private class TestData : TheoryData<ListUrlRequest>
    {
        public TestData()
        {
            Add(new(1, 1));
            Add(new(200, 1000));
        }
    }
}