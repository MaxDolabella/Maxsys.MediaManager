using System;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public record MusicCatalogInfoDTO
    {
        public Guid MusicCatalogId { get; init; }
        public string MusicCatalogName { get; init; }
    }
}