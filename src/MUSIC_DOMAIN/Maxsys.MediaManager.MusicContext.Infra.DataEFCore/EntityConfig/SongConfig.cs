using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class SongConfig : IEntityTypeConfiguration<Song>
{
    public void Configure(EntityTypeBuilder<Song> builder)
    {
        builder.ToTable("Song").HasKey(e => e.Id);

        // MediaFile
        builder.ConfigureMediaFile();

        // Properties
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Title).HasMaxLength(100).IsRequired();
        builder.Property(e => e.TrackNumber).IsRequired(false);
        builder.Property(e => e.Lyrics).HasMaxLength(5000).IsRequired(false);
        builder.Property(e => e.Comments).HasMaxLength(300).IsRequired(false);
        builder.Property(e => e.SpotifyID).HasConversion<SpotifyIDToStringValueConverter>().IsRequired(false);
        builder.Property(e => e.ISRC).IsFixedLength().HasMaxLength(12).IsRequired(false);

        // Navigation
        builder.HasOne(e => e.Album).WithMany(n => n.Songs).IsRequired();
        builder.HasMany(e => e.Composers).WithMany(n => n.Songs)
            .UsingEntity<Dictionary<string, object>>(
                $"{nameof(Composer)}{nameof(Song)}", // Join table name
                p => p.HasOne<Composer>().WithMany().HasForeignKey($"{nameof(Composer)}Id"), //[ComposerId] uniqueidentifier NOT NULL,
                p => p.HasOne<Song>().WithMany().HasForeignKey($"{nameof(Song)}Id"));   //[SongId] uniqueidentifier NOT NULL,

        // Value Objects
        builder.OwnsOne(e => e.SongDetails, oBuild =>
        {
            oBuild.Property(o => o.IsBonusTrack).IsRequired();
            oBuild.Property(o => o.VocalGender).IsRequired();
            oBuild.Property(o => o.CoveredArtist).HasMaxLength(50).IsRequired(false);
            oBuild.Property(o => o.FeaturedArtist).HasMaxLength(50).IsRequired(false);
        });

        builder.OwnsOne(e => e.SongProperties, oBuild =>
        {
            oBuild.Property(o => o.Duration).IsRequired();
            oBuild.Property(o => o.BitRate).IsRequired();
        });

        builder.OwnsOne(e => e.Classification, oBuild =>
        {
            oBuild.Property(o => o.Rating).IsRequired();
        });

        // Indexes
        //builder.HasIndex(mediaFile => mediaFile.Path).IsUnique().HasDatabaseName($"AK_Music_FullPath");
    }
}