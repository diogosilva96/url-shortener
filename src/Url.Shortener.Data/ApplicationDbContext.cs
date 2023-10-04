using Microsoft.EntityFrameworkCore;
using Url.Shortener.Data.Models;

namespace Url.Shortener.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions) { }

    // ReSharper disable once ReturnTypeCanBeEnumerable.Global
    public virtual DbSet<UrlMetadata> UrlMetadata => Set<UrlMetadata>();

    protected ApplicationDbContext() 
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}