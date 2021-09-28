using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public record AlbumListDTO
    {
        public string AlbumMusicCatalogName { get; init; }
        public string AlbumArtistName { get; init; }
        public Guid AlbumId { get; init; }
        public string AlbumName { get; init; }
        public AlbumType AlbumType { get; init; }
        public int? AlbumYear { get; init; }
        public int AlbumMusicCount { get; init; }
        public string AlbumDirectory { get; init; }
    }
}