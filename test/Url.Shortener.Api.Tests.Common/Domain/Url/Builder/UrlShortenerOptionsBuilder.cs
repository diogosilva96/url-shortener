using AutoFixture;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Tests.Common.Builder;

namespace Url.Shortener.Api.Tests.Common.Domain.Url.Builder;

internal class UrlShortenerOptionsBuilder : TestBuilderBase<CodeGeneratorOptions>
{
    protected override CodeGeneratorOptions CreateDefault()
    {
        return Fixture.Build<CodeGeneratorOptions>()
                      .With(x => x.UrlSize, 10)
                      .With(x => x.Characters, "ABCDEFGHIJKLMNOPQRSTUVWXYZ_-abcdefghijklmnopqrstuvwxyz")
                      .Create();
    }
}