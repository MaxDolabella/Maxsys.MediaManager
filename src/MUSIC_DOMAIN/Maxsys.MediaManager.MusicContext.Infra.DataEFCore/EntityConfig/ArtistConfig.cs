using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class ArtistConfig : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artist").HasKey(e => e.Id);

        // Properties
        builder.Property(p => p.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.SpotifyID).HasConversion<SpotifyIDToStringValueConverter>().IsRequired(false);

        // Navigation
        builder.HasOne(e => e.Catalog).WithMany(n => n.Artists).IsRequired();

        // Indexes
        builder.HasIndex(e => new { e.Name, e.CatalogId }).IsUnique().HasDatabaseName($"AK_Artist_Name_Catalog");
    }
}