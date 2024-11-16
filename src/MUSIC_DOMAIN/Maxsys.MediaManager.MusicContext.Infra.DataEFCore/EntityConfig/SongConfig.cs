using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class SongConfig : IEntityTypeConfiguration<Song>
{
    public void Configure(EntityTypeBuilder<Song> builder)
    {
        builder.ToTable("Songs").HasKey(song => song.Id);

        // MediaFile
        builder.Property(mediaFile => mediaFile.FullPath).IsRequired().HasMaxLength(260).HasColumnName("MediaFile_FullPath");
        builder.Property(mediaFile => mediaFile.OriginalFileName).IsRequired().HasMaxLength(100).HasColumnName("MediaFile_OriginalFileName");
        builder.Property(mediaFile => mediaFile.FileSize).IsRequired().HasColumnName("MediaFile_FileSize");
        builder.Property(mediaFile => mediaFile.CreatedDate).IsRequired().HasColumnType("datetime2").HasColumnName("MediaFile_CreatedDate");
        builder.Property(mediaFile => mediaFile.UpdatedDate).IsRequired().HasColumnType("datetime2").HasColumnName("MediaFile_UpdatedDate");

        // Properties
        builder.Property(song => song.Title).HasMaxLength(100).IsRequired();
        builder.Property(song => song.TrackNumber).IsRequired(false);
        builder.Property(song => song.Lyrics).HasMaxLength(5000).IsRequired(false);
        builder.Property(song => song.Comments).HasMaxLength(300).IsRequired(false);

        // Navigation
        builder.HasOne(song => song.Album).WithMany(album => album.Songs);
        builder.HasMany(song => song.Composers).WithMany(composer => composer.Songs);

        // Value Objects
        builder.OwnsOne(song => song.SongDetails, valueObj =>
        {
            valueObj.Property(songDetails => songDetails.IsBonusTrack).IsRequired();
            valueObj.Property(songDetails => songDetails.VocalGender).IsRequired();
            valueObj.Property(songDetails => songDetails.CoveredArtist).HasMaxLength(50).IsRequired(false);
            valueObj.Property(songDetails => songDetails.FeaturedArtist).HasMaxLength(50).IsRequired(false);
        });

        builder.OwnsOne(song => song.SongProperties, valueObj =>
        {
            valueObj.Property(songProperties => songProperties.Duration).IsRequired();
            valueObj.Property(songProperties => songProperties.BitRate).IsRequired();
        });

        builder.OwnsOne(song => song.Classification, valueObj =>
        {
            valueObj.Property(classification => classification.Rating).IsRequired();
        });

        // Indexes
        builder.HasIndex(mediaFile => mediaFile.FullPath).IsUnique().HasDatabaseName($"AK_Musics_FullPath");
    }
}