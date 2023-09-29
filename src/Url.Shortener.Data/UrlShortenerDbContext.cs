using Microsoft.EntityFrameworkCore;
using Url.Shortener.Data.Models;

namespace Url.Shortener.Data;

public class UrlShortenerDbContext : DbContext
{
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> dbContextOptions) : base(dbContextOptions) { }

    // ReSharper disable once ReturnTypeCanBeEnumerable.Global
    public virtual DbSet<UrlMetadata> UrlMetadata => Set<UrlMetadata>();

    protected UrlShortenerDbContext() 
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UrlShortenerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}