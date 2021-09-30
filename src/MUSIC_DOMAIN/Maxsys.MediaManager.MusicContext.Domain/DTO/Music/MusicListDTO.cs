using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public record MusicListDTO
    {
        public Guid MusicId { get; init; }
        public string MusicFullPath { get; init; }
        public string MusicCatalogName { get; init; }
        public string AlbumName { get; init; }
        public AlbumType AlbumType { get; init; }
        public string ArtistName { get; init; }
        public int? MusicTrackNumber { get; init; }
        public string MusicTitle { get; init; }
        public int MusicRating { get; init; }
        public VocalGender MusicVocalGender { get; init; }
        public string MusicCoveredArtist { get; init; }
        public string MusicFeaturedArtist { get; init; }
    }
}