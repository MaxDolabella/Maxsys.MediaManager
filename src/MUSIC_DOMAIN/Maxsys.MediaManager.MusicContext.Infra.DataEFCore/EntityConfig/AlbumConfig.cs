using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class AlbumConfig : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("Albums");

        builder.HasKey(album => album.Id);

        // Properties
        builder.Property(album => album.AlbumDirectory)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(album => album.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(album => album.Year)
            .IsRequired(false);

        builder.Property(album => album.Genre)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(album => album.AlbumType)
            .IsRequired();

        builder.Property(album => album.AlbumCover)
            .IsRequired(); // If doesn't exits, byte[] must be empty

        // Navigation
        builder.HasOne(album => album.Artist)
            .WithMany(artist => artist.Albums);

        // Indexes
        builder.HasIndex(album => new { album.ArtistId, album.Name })
            .IsUnique().HasDatabaseName($"AK_Albums_ArtistAlbumName");
    }
}