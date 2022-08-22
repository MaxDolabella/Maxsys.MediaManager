namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct ArtistInfoDTO(Guid ArtistId, Guid CatalogId, string ArtistName, string CatalogName, int AlbumsCount);