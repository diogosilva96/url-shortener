using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Url.Shortener.Api.Domain.Url.Create;

public class CodeGenerator : ICodeGenerator
{
    private readonly char[] _characters;
    private readonly int _urlSize;

    public CodeGenerator(IOptions<CodeGeneratorOptions> options)
    {
        _characters = options.Value.Characters.ToCharArray();
        _urlSize = options.Value.UrlSize;
    }

    public string GenerateCode()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < _urlSize; i++)
        {
            var characterIndex = RandomNumberGenerator.GetInt32(0, _characters.Length);
            sb.Append(_characters[characterIndex]);
        }

        return sb.ToString();
    }
}