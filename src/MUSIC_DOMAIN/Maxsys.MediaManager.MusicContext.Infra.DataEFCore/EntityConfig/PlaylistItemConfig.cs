using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class PlaylistItemConfig : IEntityTypeConfiguration<PlaylistItem>
{
    public void Configure(EntityTypeBuilder<PlaylistItem> builder)
    {
        builder.ToTable("PlaylistItems").HasKey(playlistItem => new { playlistItem.PlaylistId, playlistItem.SongId });

        // Properties
        builder.Property(playlistItem => playlistItem.Order).IsRequired(false);

        // Navigation
        builder.HasOne(playlistItem => playlistItem.Song).WithMany();
        builder.HasOne(playlistItem => playlistItem.Playlist).WithMany(playlist => playlist.Items);
    }
}