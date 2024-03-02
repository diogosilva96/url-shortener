using Microsoft.Extensions.Logging;
using NSubstitute;
using Url.Shortener.Api.Domain.Url.Create;
using Url.Shortener.Api.UnitTests.Data.Builder;
using Url.Shortener.Data;

namespace Url.Shortener.Api.UnitTests.Domain.Url.Create.Builder;

public class CreateUrlRequestHandlerBuilder
{
    private ICodeGenerator _codeGenerator;
    private ApplicationDbContext _dbContext;
    private ILogger<CreateUrlRequestHandler> _logger;
    private TimeProvider _timeProvider;

    public CreateUrlRequestHandlerBuilder()
    {
        _dbContext = new UrlShortenerDbContextBuilder().Build();
        _codeGenerator = Substitute.For<ICodeGenerator>();
        _timeProvider = Substitute.For<TimeProvider>();
        _logger = Substitute.For<ILogger<CreateUrlRequestHandler>>();
    }

    public CreateUrlRequestHandler Build() => new(_dbContext, _codeGenerator, _timeProvider, _logger);

    public CreateUrlRequestHandlerBuilder With(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        return this;
    }

    public CreateUrlRequestHandlerBuilder With(ICodeGenerator codeGenerator)
    {
        _codeGenerator = codeGenerator;

        return this;
    }

    public CreateUrlRequestHandlerBuilder With(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;

        return this;
    }

    public CreateUrlRequestHandlerBuilder With(ILogger<CreateUrlRequestHandler> logger)
    {
        _logger = logger;

        return this;
    }
}