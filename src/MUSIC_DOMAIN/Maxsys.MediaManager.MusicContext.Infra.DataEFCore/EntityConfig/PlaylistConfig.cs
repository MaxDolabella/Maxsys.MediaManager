using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class PlaylistConfig : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.ToTable("Playlist").HasKey(e => e.Id);

        // Properties
        builder.Property(p => p.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.SpotifyID).HasMaxLength(50).IsRequired(false);

        // Navigation
        // builder.HasMany(e => e.Items).WithOne(item => item.Playlist);

        // Indexes
        builder.HasIndex(e => e.Name).IsUnique().HasDatabaseName($"AK_Playlist_Name");
    }
}