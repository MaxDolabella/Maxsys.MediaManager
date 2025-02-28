using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class AlbumConfig : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("Album").HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Directory).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Year).IsRequired(false);
        builder.Property(e => e.Genre).HasMaxLength(50).IsRequired();
        builder.Property(e => e.AlbumType).IsRequired();
        builder.Property(e => e.AlbumCover).IsRequired(); // If doesn't exits, byte[] must be empty
        builder.Property(e => e.SpotifyID).HasMaxLength(50).IsRequired(false);

        // Navigation
        builder.HasOne(e => e.Artist).WithMany(n => n.Albums).IsRequired();

        // Indexes
        builder.HasIndex(e => new { e.ArtistId, e.Name }).IsUnique().HasDatabaseName($"AK_Album_ArtistAlbumName");
    }
}