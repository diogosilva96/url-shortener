using AutoFixture;
using Url.Shortener.Api.Tests.Common.Builder;
using Url.Shortener.Data.Models;

namespace Url.Shortener.Api.UnitTests.Data.Builder;

internal class UrlMetadataBuilder : TestBuilderBase<UrlMetadata>
{
    protected override UrlMetadata CreateDefault() =>
        Fixture.Build<UrlMetadata>()
               .With(x => x.Code, Fixture.Create<Uri>().ToString)
               .With(x => x.FullUrl, Fixture.Create<Uri>().ToString)
               .Create();
}