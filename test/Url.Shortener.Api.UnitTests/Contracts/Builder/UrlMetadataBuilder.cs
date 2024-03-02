using AutoFixture;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.Tests.Common.Builder;

namespace Url.Shortener.Api.UnitTests.Contracts.Builder;

public class UrlMetadataBuilder : TestBuilderBase<UrlMetadata>
{
    protected override UrlMetadata CreateDefault() =>
        Fixture.Build<UrlMetadata>()
               .With(x => x.Code, Fixture.Create<Uri>().ToString)
               .With(x => x.FullUrl, Fixture.Create<Uri>().ToString)
               .Create();
}