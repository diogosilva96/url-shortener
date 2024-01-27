﻿using AutoFixture;
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
    public async Task ThenNoExceptionIsThrown(string url, string? code)
    {
        var exception = await Record.ExceptionAsync(() => WhenValidatingAsync(url, code));

        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public async Task ThenTheValidationSucceeds(string url, string? code)
    {
        var validationResult = await WhenValidatingAsync(url, code);

        Assert.True(validationResult.IsValid);
    }

    private async Task<ValidationResult> WhenValidatingAsync(string url, string? code)
    {
        var request = new CreateUrlRequest(url, code);
        return await _validator.ValidateAsync(request);
    }

    private class TestData : TheoryData<string, string?>
    {
        public TestData()
        {
            var fixture = new Fixture();
            var validUrl = $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}";
            Add(validUrl, default);
            Add(validUrl, "abcde");                                              // 5 length string
            Add(validUrl, "tAJqPRr@ywgK5Av+nzu6c@WgB5AfxWj4Mmnt5o3UuqEqYFsARJ"); // 50 length string
        }
    }
}