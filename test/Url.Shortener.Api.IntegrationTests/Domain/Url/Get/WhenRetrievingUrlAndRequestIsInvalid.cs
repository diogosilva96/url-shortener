using AutoFixture;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.Get;

public sealed class WhenRetrievingUrlAndRequestIsInvalid : IntegrationTestBase
{
    private readonly string _shortUrl;
    private readonly IntegrationTestWebApplicationFactory _webApplicationFactory;

    public WhenRetrievingUrlAndRequestIsInvalid(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        var fixture = new Fixture();

        var metadata = new[]
        {
            new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                    .Build()
        };

        _shortUrl = "  ";


        _webApplicationFactory = webApplicationFactory;

        webApplicationFactory.SeedData(context => context.UrlMetadata.AddRange(metadata));
    }

    [Fact]
    public async Task ThenAnExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRetrievingAsync);

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task ThenAValidationExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRetrievingAsync);

        Assert.IsType<ValidationException>(exception);
    }

    private async Task<IResult> WhenRetrievingAsync() =>
        await UrlEndpoints.GetUrl(_shortUrl, _webApplicationFactory.GetRequiredService<IMediator>());
}