using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class PlaylistItemConfig : IEntityTypeConfiguration<PlaylistItem>
{
    public void Configure(EntityTypeBuilder<PlaylistItem> builder)
    {
        builder.ToTable("PlaylistItem").HasKey(e => new { e.PlaylistId, e.SongId });

        // Properties
        builder.Property(e => e.PlaylistId).IsRequired();
        builder.Property(e => e.SongId).IsRequired();
        builder.Property(e => e.Order).IsRequired(false);

        // Navigation
        builder.HasOne(e => e.Song).WithMany().IsRequired();
        builder.HasOne(e => e.Playlist).WithMany(n => n.Items).IsRequired();
    }
}