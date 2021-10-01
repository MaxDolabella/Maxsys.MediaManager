using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig
{
    internal class PlaylistItemConfig : IEntityTypeConfiguration<PlaylistItem>
    {
        public void Configure(EntityTypeBuilder<PlaylistItem> builder)
        {
            builder.ToTable("PlaylistItems");

            builder.HasKey(playlistItem => new { playlistItem.PlaylistId, playlistItem.MusicId });

            builder.Property(playlistItem => playlistItem.Order).IsRequired();

            builder.HasOne(playlistItem => playlistItem.Music);

            builder.HasOne(playlistItem => playlistItem.Playlist)
                .WithMany(playlist => playlist.Items);
        }
    }
}