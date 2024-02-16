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
    public async Task ThenNoExceptionIsThrown(int pageSize, int page)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(pageSize, page));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationSucceeds(int pageSize, int page)
    {
        var validationResult = await WhenValidatingAsync(pageSize, page);

        Assert.True(validationResult.IsValid);
    }

    private async Task<ValidationResult> WhenValidatingAsync(int pageSize, int page)
    {
        var request = new ListUrlRequest(pageSize, page);
        return await _validator.ValidateAsync(request);
    }

    private class TestData : TheoryData<int, int>
    {
        public TestData()
        {
            Add(1, 1);
            Add(200, 1000);
        }
    }
}