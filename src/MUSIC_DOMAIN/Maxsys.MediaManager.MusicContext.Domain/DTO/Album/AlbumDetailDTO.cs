namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct AlbumDetailDTO(Guid AlbumId, string CatalogName, string ArtistName, string AlbumName, AlbumType AlbumType, int? AlbumYear, int AlbumMusicCount, string AlbumDirectory);