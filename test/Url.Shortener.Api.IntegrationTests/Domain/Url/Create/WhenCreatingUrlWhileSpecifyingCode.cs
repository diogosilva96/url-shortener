﻿using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.Create;

public sealed class WhenCreatingUrlWhileSpecifyingCode : IntegrationTestBase
{
    private readonly CreateUrlRequest _request;

    public WhenCreatingUrlWhileSpecifyingCode(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        var fixture = new Fixture();

        _request = new()
        {
            Url = $"https://{fixture.Create<string>()}.com/",
            Code = fixture.Create<string>()
        };

        var metadata = new[]
        {
            new UrlMetadataBuilder().With(x => x.Code = fixture.Create<string>()[..15])
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.Code = fixture.Create<string>()[..15])
                                    .Build()
        };

        webApplicationFactory.SeedData(context => context.UrlMetadata.AddRange(metadata));
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenCreatingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenAnOkResponseIsReturned()
    {
        var response = await WhenCreatingAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ThenTheExpectedCodeIsReturned()
    {
        var response = await WhenCreatingAsync();

        var code = await response.Content.ReadAsStringAsync();

        Assert.Contains(_request.Code!, code);
    }

    private async Task<HttpResponseMessage> WhenCreatingAsync() =>
        await Client.PostAsync(Urls.Api.Urls.Create, JsonContent.Create(_request));
}