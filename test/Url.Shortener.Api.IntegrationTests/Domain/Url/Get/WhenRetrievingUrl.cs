using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Url.Shortener.Api.Domain.Url;
using Url.Shortener.Api.IntegrationTests.Data.Builder;
using Url.Shortener.Api.IntegrationTests.Utils;
using Xunit;

namespace Url.Shortener.Api.IntegrationTests.Domain.Url.Get;

public sealed class WhenRetrievingUrl : IntegrationTestBase
{
    private readonly string _expectedRedirectUrl;
    private readonly string _shortUrl;
    private readonly IntegrationTestWebApplicationFactory _webApplicationFactory;

    public WhenRetrievingUrl(IntegrationTestWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        var fixture = new Fixture();
        var urlMetadata = new UrlMetadataBuilder().With(x => x.ShortUrl = fixture.Create<string>()[..15])
                                                  .Build();
        _shortUrl = urlMetadata.ShortUrl;
        _expectedRedirectUrl = urlMetadata.FullUrl;

        _webApplicationFactory = webApplicationFactory;

        webApplicationFactory.SeedData(context => context.Add(urlMetadata));
    }

    [Fact]
    public async Task ThenNoExceptionIsThrown()
    {
        var exception = await Record.ExceptionAsync(WhenRetrievingAsync);

        Assert.Null(exception);
    }

    [Fact]
    public async Task ThenAnRedirectResultIsReturned()
    {
        var result = await WhenRetrievingAsync();

        Assert.IsType<RedirectHttpResult>(result);
    }

    [Fact]
    public async Task ThenARedirectResultIsReturnedForTheExpectedUrl()
    {
        var result = await WhenRetrievingAsync();

        var redirectResult = (RedirectHttpResult)result;
        Assert.Equal(_expectedRedirectUrl, redirectResult.Url);
    }

    private async Task<IResult> WhenRetrievingAsync() =>
        await UrlEndpoints.GetUrl(_shortUrl, _webApplicationFactory.GetRequiredService<IMediator>());
}