namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct ArtistDetailsDTO(Guid ArtistId, Guid CatalogId, string ArtistName, string CatalogName);