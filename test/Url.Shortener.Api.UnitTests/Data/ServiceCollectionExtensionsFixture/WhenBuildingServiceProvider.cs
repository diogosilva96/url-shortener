using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Url.Shortener.Api.Data;
using Url.Shortener.Api.UnitTests.Builder;
using Url.Shortener.Data;
using Xunit;

namespace Url.Shortener.Api.UnitTests.Data.ServiceCollectionExtensionsFixture;

public class WhenBuildingServiceProvider
{
    private readonly string _connectionString;
    private readonly IServiceCollection _serviceCollection;

    public WhenBuildingServiceProvider()
    {
        var fixture = new Fixture();
        _connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = fixture.Create<string>(),
            Database = fixture.Create<string>(),
            Username = fixture.Create<string>(),
            Password = fixture.Create<string>()
        }.ToString();

        _serviceCollection = ServiceCollectionBuilder.Build();
    }

    [Fact]
    public void ThenNoExceptionIsThrown()
    {
        var exception = Record.Exception(WhenBuilding);

        Assert.Null(exception);
    }

    [Fact]
    public void ThenAnApplicationDbContextCanBeRetrieved()
    {
        var provider = WhenBuilding();

        Assert.NotNull(provider.GetService<ApplicationDbContext>());
    }

    private IServiceProvider WhenBuilding() => _serviceCollection.AddDataServices(_connectionString)
                                                                 .BuildServiceProvider();
}