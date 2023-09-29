using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Url.Shortener.Data.Models;

namespace Url.Shortener.Data.Configuration;

internal class UrlMetadataConfiguration : IEntityTypeConfiguration<UrlMetadata>
{
    public void Configure(EntityTypeBuilder<UrlMetadata> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ShortUrl)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(x => x.FullUrl)
               .HasMaxLength(2048) // max length 2048 just to be safe in different browsers, as the length depends on the browser used.
               .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
               .IsRequired();

        builder.HasIndex(x => x.ShortUrl)
               .IsUnique();
    }
}