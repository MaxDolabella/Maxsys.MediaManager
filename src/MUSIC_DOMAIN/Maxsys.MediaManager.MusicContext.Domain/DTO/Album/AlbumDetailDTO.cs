using Maxsys.MediaManager.MusicContext.Domain.Enums;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct AlbumDetailDTO(Guid AlbumId, string CatalogName, string ArtistName, string AlbumName, AlbumTypes AlbumType, int? AlbumYear, int AlbumMusicCount, string AlbumDirectory);