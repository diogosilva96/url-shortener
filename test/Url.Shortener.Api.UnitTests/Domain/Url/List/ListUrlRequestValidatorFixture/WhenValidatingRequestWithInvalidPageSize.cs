using FluentValidation.Results;
using Url.Shortener.Api.Domain.Url.List;
using Url.Shortener.Api.UnitTests.Domain.Url.List.Builder;
using Xunit;
using ListUrlRequest = Url.Shortener.Api.Domain.Url.List.ListUrlRequest;

namespace Url.Shortener.Api.UnitTests.Domain.Url.List.ListUrlRequestValidatorFixture;

public class WhenValidatingRequestWithInvalidPageSize
{
    private readonly ListUrlRequestValidator _validator;

    public WhenValidatingRequestWithInvalidPageSize() => _validator = ListUrlRequestValidatorBuilder.Build();

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenNoExceptionIsThrown(int pageSize)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(pageSize));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFails(int pageSize)
    {
        var validationResult = await WhenValidatingAsync(pageSize);

        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationFailsForThePageSize(int pageSize)
    {
        var validationResult = await WhenValidatingAsync(pageSize);

        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(ListUrlRequest.PageSize));
    }

    private async Task<ValidationResult> WhenValidatingAsync(int pageSize)
    {
        var request = new ListUrlRequest(pageSize, 10);
        return await _validator.ValidateAsync(request);
    }

    private class TestData : TheoryData<int>
    {
        public TestData()
        {
            Add(-1);
            Add(0);
            Add(201);
        }
    }
}