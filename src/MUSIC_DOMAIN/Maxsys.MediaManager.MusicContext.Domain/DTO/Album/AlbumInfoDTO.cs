using System;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public record AlbumInfoDTO
    {
        public Guid AlbumId { get; init; }
        public Guid ArtistId { get; init; }
        public string AlbumName { get; init; }
        public string ArtistName { get; init; }
        public string AlbumDirectory { get; init; }
    }
}