namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct SongInfoDTO(Guid AlbumId, Guid SongId, string AlbumName, int? SongTrackNumber, string SongTitle);