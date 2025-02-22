using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class ArtistConfig : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists").HasKey(artist => artist.Id);

        // Properties
        builder.Property(artist => artist.Name).HasMaxLength(50).IsRequired();
        builder.Property(artist => artist.SpotifyId).HasMaxLength(50).IsRequired(false);

        // Navigation
        builder.HasOne(artist => artist.Catalog).WithMany(catalog => catalog.Artists);

        // Indexes
        builder.HasIndex(artist => new { artist.Name, artist.CatalogId }).IsUnique().HasDatabaseName($"AK_Artists_Name_Catalog");
    }
}