using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.Tests.Common.Domain.Url.Builder;
using Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.CodeGeneratorFixture;

public class WhenGeneratingCode
{
    private readonly CodeGeneratorOptions _options;
    private readonly CodeGenerator _codeGenerator;

    public WhenGeneratingCode()
    {
        _options = new UrlShortenerOptionsBuilder().With(x => x.UrlSize = 10)
                                                   .With(x => x.Characters = "ABCabc_-")
                                                   .Build();

        _codeGenerator = new UrlShortenerBuilder().With(_options)
                                                 .Build();
    }

    [Fact]
    public void ThenNoExceptionIsThrown()
    {
        var exception = Record.Exception(WhenGenerating);

        Assert.Null(exception);
    }

    [Fact]
    public void ThenACodeIsReturned()
    {
        var code = WhenGenerating();

        Assert.NotEmpty(code);
    }

    [Fact]
    public void ThenACodeIsReturnedWithTheExpectedSize()
    {
        var code = WhenGenerating();

        Assert.Equal(_options.UrlSize, code.Length);
    }

    [Fact]
    public void ThenACodeIsReturnedWithinTheExpectedCharactersRange()
    {
        var code = WhenGenerating();

        Assert.All(code, character => Assert.Contains(character, _options.Characters));
    }

    private string WhenGenerating() => _codeGenerator.GenerateCode();
}