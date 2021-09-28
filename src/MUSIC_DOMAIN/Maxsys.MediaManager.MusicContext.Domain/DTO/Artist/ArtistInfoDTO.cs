using System;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public record ArtistInfoDTO
    {
        public Guid ArtistId { get; init; }
        public Guid MusicCatalogId { get; init; }
        public string ArtistName { get; init; }
        public string MusicCatalogName { get; init; }
    }
}