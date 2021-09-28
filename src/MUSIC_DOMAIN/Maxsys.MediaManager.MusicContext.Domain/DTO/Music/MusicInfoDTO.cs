using System;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public record MusicInfoDTO
    {
        public Guid AlbumId { get; init; }
        public string AlbumName { get; init; }

        public Guid MusicId { get; init; }
        public int? MusicTrack { get; init; }
        public string MusicTitle { get; init; }
    }
}