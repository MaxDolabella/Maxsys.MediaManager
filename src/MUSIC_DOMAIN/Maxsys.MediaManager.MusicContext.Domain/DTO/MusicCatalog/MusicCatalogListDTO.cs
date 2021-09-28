using System;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public record MusicCatalogListDTO
    {
        public Guid MusicCatalogId { get; init; }
        public string MusicCatalogName { get; init; }
        public int ArtistCount { get; init; }
    }
}