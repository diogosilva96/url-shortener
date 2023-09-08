using Microsoft.EntityFrameworkCore;
using Url.Shortener.Data.Models;

namespace Url.Shortener.Data;

public class UrlShortenerDbContext : DbContext
{
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> dbContextOptions) : base(dbContextOptions) { }

    public DbSet<UrlMetadata> UrlMetadata => Set<UrlMetadata>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UrlShortenerDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}