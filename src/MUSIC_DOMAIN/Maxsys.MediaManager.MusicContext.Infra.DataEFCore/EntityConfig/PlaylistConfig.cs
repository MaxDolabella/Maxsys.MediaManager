using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class PlaylistConfig : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.ToTable("Playlists").HasKey(playlist => playlist.Id);

        // Properties
        builder.Property(playlist => playlist.Name).HasMaxLength(50).IsRequired();

        // Navigation
        builder.HasMany(playlist => playlist.Items).WithOne(item => item.Playlist);

        // Indexes
        builder.HasIndex(playlist => playlist.Name).IsUnique().HasDatabaseName($"AK_Playlists_Name");
    }
}