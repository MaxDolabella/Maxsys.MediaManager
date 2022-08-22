namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct CatalogInfoDTO(Guid MusicCatalogId, string MusicCatalogName, int ArtistCount);