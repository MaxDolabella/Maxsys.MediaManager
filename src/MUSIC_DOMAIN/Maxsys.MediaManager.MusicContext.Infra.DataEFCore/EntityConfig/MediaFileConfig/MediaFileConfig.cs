using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal static class MediaFileConfig
{
    public static void ConfigureMediaFile<T>(this EntityTypeBuilder<T> builder)
        where T : MediaFile
    {
        builder.Property(mediaFile => mediaFile.FullPath).HasMaxLength(260).HasColumnName("MediaFile_FullPath").IsRequired();
        builder.Property(mediaFile => mediaFile.OriginalFileName).HasMaxLength(100).HasColumnName("MediaFile_OriginalFileName").IsRequired();
        builder.Property(mediaFile => mediaFile.FileSize).HasColumnName("MediaFile_FileSize").IsRequired();
        builder.Property(mediaFile => mediaFile.CreatedAt).HasColumnType("datetime2").HasColumnName("MediaFile_CreatedAt").IsRequired();
        builder.Property(mediaFile => mediaFile.LastUpdateAt).HasColumnType("datetime2").HasColumnName("MediaFile_LastUpdateAt").IsRequired(false);
    }
}