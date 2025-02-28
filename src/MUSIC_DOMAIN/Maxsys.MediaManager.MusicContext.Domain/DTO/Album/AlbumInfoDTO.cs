namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct AlbumInfoDTO(Guid AlbumId, Guid ArtistId, string AlbumName, string ArtistName, Uri AlbumDirectory);