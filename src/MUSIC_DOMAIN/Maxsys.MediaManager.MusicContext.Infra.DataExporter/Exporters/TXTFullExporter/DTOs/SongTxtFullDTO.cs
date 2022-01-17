using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using System;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.Exporters.TXTFullExporter.DTOs
{
    internal record SongTxtFullDTO(
        Guid SongId,
        string SongFullPath,
        int? SongNumber,
        string SongTitle,
        string SongComments,
        bool SongIsBonusTrack,
        VocalGender SongVocalGender,
        string SongCoveredArtist,
        string SongFeaturedArtist,
        int SongRating,
        byte SongStars10,
        TimeSpan SongDuration,
        int SongBitRate,
        Guid AlbumId,
        int? AlbumYear,
        string AlbumName,
        string AlbumGenre,
        AlbumType AlbumType,
        Guid ArtistId,
        string ArtistName,
        Guid CatalogId,
        string CatalogName);
}