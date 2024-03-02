using AutoFixture;
using Url.Shortener.Api.Contracts;
using Url.Shortener.Api.Tests.Common.Builder;

namespace Url.Shortener.Api.UnitTests.Contracts.Builder;

public class PagedResultBuilder<T> : TestBuilderBase<PagedResult<T>>
{
    protected override PagedResult<T> CreateDefault() => Fixture.Build<PagedResult<T>>()
                                                                .With(x => x.Data, Fixture.CreateMany<T>().ToList())
                                                                .Create();
}