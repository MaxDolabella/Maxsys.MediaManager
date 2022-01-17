using System;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.Exporters.TXTExporter.DTOs
{
    internal record SongTxtDTO(
        Guid SongId,
        string SongFullPath,
        int? SongNumber,
        string SongTitle,
        int SongRating,
        byte SongStars10,
        TimeSpan SongDuration,
        Guid AlbumId,
        int? AlbumYear,
        string AlbumName,
        string AlbumGenre,
        Guid ArtistId,
        string ArtistName,
        Guid CatalogId,
        string CatalogName);
}