using AutoFixture;
using Url.Shortener.Api.Tests.Common.Builder;
using Url.Shortener.Data.Models;

namespace Url.Shortener.Api.IntegrationTests.Data.Builder;

public class UrlMetadataBuilder : TestBuilderBase<UrlMetadata>
{
    protected override UrlMetadata CreateDefault() =>
        Fixture.Build<UrlMetadata>()
               .With(x => x.Code, Fixture.Create<Uri>().ToString)
               .With(x => x.FullUrl, $"https://{Fixture.Create<string>()}.com/")
               .With(x => x.CreatedAtUtc, DateTimeOffset.UtcNow)
               .Create();
}