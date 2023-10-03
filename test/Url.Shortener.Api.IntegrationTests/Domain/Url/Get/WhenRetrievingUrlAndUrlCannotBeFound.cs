using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.Exceptions;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.Get;

public sealed class WhenRetrievingUrlAndUrlCannotBeFound : IntegrationTestBase
{
    private readonly HttpClient _client;
    private readonly string _shortUrl;
    private readonly IntegrationTestWebApplicationFactory _webApplicationFactory;

    public WhenRetrievingUrlAndUrlCannotBeFound(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        var fixture = new Fixture();

        var metadata = new[]
        {
            new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                    .Build(),
            new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                    .Build()
        };

        _shortUrl = fixture.Create<string>()[..5];


        _webApplicationFactory = webApplicationFactory;

        webApplicationFactory.SeedData(context => context.UrlMetadata.AddRange(metadata));

        _client = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task ThenAnExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRetrievingAsync);

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task ThenAnNotFoundExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRetrievingAsync);

        Assert.IsType<NotFoundException>(exception);
    }

    private async Task<IResult> WhenRetrievingAsync() =>
        await UrlEndpoints.GetUrl(_shortUrl, _webApplicationFactory.GetRequiredService<IMediator>());
}