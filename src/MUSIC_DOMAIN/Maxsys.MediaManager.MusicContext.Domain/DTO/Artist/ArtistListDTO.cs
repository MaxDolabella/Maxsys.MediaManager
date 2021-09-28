using System;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public record ArtistListDTO
    {
        public Guid ArtistId { get; init; }
        public string ArtistName { get; init; }
        public string MusicCatalogName { get; init; }
        public int AlbumsCount{ get; init; }
    }
}