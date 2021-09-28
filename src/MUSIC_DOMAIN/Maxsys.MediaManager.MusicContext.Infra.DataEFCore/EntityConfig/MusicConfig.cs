using Maxsys.MediaManager.CoreDomain;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig
{
    internal class MusicConfig : IEntityTypeConfiguration<Music>
    {
        public void Configure(EntityTypeBuilder<Music> builder)
        {
            builder.ToTable("Musics").HasKey(music => music.Id);

            #region MediaFile

            //Guid id, string path, string originalName, long fileSize, DateTime addedDate
            builder.Property(mediaFile => mediaFile.FullPath)
                .IsRequired()
                .HasMaxLength(260)
                .HasColumnName($"{nameof(MediaFile)}_{nameof(MediaFile.FullPath)}");

            builder.Property(mediaFile => mediaFile.OriginalFileName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName($"{nameof(MediaFile)}_{nameof(MediaFile.OriginalFileName)}");

            builder.Property(mediaFile => mediaFile.FileSize)
                .IsRequired()
                .HasColumnName($"{nameof(MediaFile)}_{nameof(MediaFile.FileSize)}");

            builder.Property(mediaFile => mediaFile.CreatedDate)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasColumnName($"{nameof(MediaFile)}_{nameof(MediaFile.CreatedDate)}");

            builder.Property(mediaFile => mediaFile.UpdatedDate)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasColumnName($"{nameof(MediaFile)}_{nameof(MediaFile.UpdatedDate)}");

            #endregion MediaFile

            #region Properties

            builder.Property(music => music.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(music => music.TrackNumber)
                .IsRequired(false);

            builder.Property(music => music.Lyrics)
                .HasMaxLength(5000)
                .IsRequired(false);

            builder.Property(music => music.Comments)
                .HasMaxLength(300)
                .IsRequired(false);

            #endregion Properties

            #region Navigation

            // One-to-many
            builder.HasOne(music => music.Album)
                .WithMany(album => album.Musics);

            // many-to-many
            builder.HasMany(music => music.Composers)
                .WithMany(composer => composer.Musics);

            #endregion Navigation

            #region Value Objects

            builder.OwnsOne(music => music.MusicDetails, valueObj =>
            {
                valueObj.Property(musicDetails => musicDetails.IsBonusTrack)
                    .IsRequired();

                valueObj.Property(musicDetails => musicDetails.VocalGender)
                    .IsRequired();

                valueObj.Property(musicDetails => musicDetails.CoveredArtist)
                    .HasMaxLength(50)
                    .IsRequired(false);

                valueObj.Property(musicDetails => musicDetails.FeaturedArtist)
                    .HasMaxLength(50)
                    .IsRequired(false);
            });

            builder.OwnsOne(music => music.MusicProperties, valueObj =>
            {
                valueObj.Property(musicProperties => musicProperties.Duration)
                    .IsRequired();

                valueObj.Property(musicProperties => musicProperties.BitRate)
                    .IsRequired();
            });

            builder.OwnsOne(music => music.Classification, valueObj =>
            {
                valueObj.Property(classification => classification.Rating)
                    .IsRequired();
            });

            #endregion Value Objects

            #region Indexes

            builder.HasIndex(mediaFile => mediaFile.FullPath)
                .IsUnique().HasDatabaseName($"AK_Musics_FullPath");

            #endregion Indexes
        }
    }
}