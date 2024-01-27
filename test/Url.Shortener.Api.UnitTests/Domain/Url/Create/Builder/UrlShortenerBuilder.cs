using Microsoft.Extensions.Options;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Tests.Common.Domain.Url.Builder;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;

internal class UrlShortenerBuilder
{
    private readonly IOptions<CodeGeneratorOptions> _options;

    public UrlShortenerBuilder()
    {
        _options = Substitute.For<IOptions<CodeGeneratorOptions>>();
        AssignOptions(new UrlShortenerOptionsBuilder().Build());
    }

    public CodeGenerator Build() => new(_options);

    public UrlShortenerBuilder With(CodeGeneratorOptions options)
    {
        AssignOptions(options);

        return this;
    }

    private void AssignOptions(CodeGeneratorOptions options) => _options.Value.Returns(options);
}