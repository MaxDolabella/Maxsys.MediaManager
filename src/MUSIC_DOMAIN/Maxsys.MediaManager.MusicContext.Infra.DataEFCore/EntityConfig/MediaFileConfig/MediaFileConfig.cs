using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal static class MediaFileConfig
{
    public static void ConfigureMediaFile<T>(this EntityTypeBuilder<T> builder)
        where T : MediaFile
    {
        builder.Property(mediaFile => mediaFile.Path).HasColumnName("MediaFile_Path")/*.HasConversion<SystemPathToStringValueConverter>()*/.HasMaxLength(255).IsRequired();
        builder.Property(mediaFile => mediaFile.OriginalFile).HasColumnName("MediaFile_OriginalFile")/*.HasConversion<SystemPathToStringValueConverter>()*/.IsRequired();
        builder.Property(mediaFile => mediaFile.FileSize).HasColumnName("MediaFile_FileSize").IsRequired();
        builder.Property(mediaFile => mediaFile.CreatedAt).HasColumnType("datetime2").HasColumnName("MediaFile_CreatedAt").IsRequired();
        builder.Property(mediaFile => mediaFile.LastUpdateAt).HasColumnType("datetime2").HasColumnName("MediaFile_LastUpdateAt").IsRequired(false);
    }
}