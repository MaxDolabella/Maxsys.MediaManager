namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct SongDetailDTO(
    Guid CatalogId,
    Guid ArtistId,
    Guid AlbumId,
    Guid SongId,
    string CatalogName,
    string ArtistName,
    string AlbumName,
    AlbumType AlbumType,
    string SongFullPath,
    int? SongTrackNumber,
    string SongTitle,
    int SongRating,
    VocalGender SongVocalGender,
    string? SongCoveredArtist,
    string? SongFeaturedArtist);