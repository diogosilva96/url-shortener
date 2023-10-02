using AutoFixture;
using Url.Shortener.Api.Tests.Common.Builder;
using Url.Shortener.Data.Models;

namespace Url.Shortener.Api.IntegrationTests.Data.Builder;

internal class UrlMetadataBuilder : TestBuilderBase<UrlMetadata>
{
    protected override UrlMetadata CreateDefault() =>
        Fixture.Build<UrlMetadata>()
               .With(x => x.ShortUrl, Fixture.Create<Uri>().ToString)
               .With(x => x.FullUrl, Fixture.Create<string>())
               .With(x => x.CreatedAtUtc, DateTimeOffset.UtcNow)
               .Create();
}