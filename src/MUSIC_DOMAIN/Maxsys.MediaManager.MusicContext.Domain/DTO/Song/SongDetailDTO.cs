using Maxsys.MediaManager.MusicContext.Domain.Enums;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct SongDetailDTO(
    Guid CatalogId,
    Guid ArtistId,
    Guid AlbumId,
    Guid SongId,
    string CatalogName,
    string ArtistName,
    string AlbumName,
    AlbumTypes AlbumType,
    Uri SongFullPath,
    int? SongTrackNumber,
    string SongTitle,
    int SongRating,
    VocalGenders SongVocalGender,
    string? SongCoveredArtist,
    string? SongFeaturedArtist);