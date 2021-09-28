namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public struct DefineMusicFileNameDTO
    {
        public int? MusicTrackNumber { get; init; }
        public string MusicTitle { get; init; }
        public string MusicCoveredArtist { get; init; }
        public string MusicFeaturedArtist { get; init; }
        public bool MusicIsBonusTrack { get; init; }
        public string AlbumDirectory { get; set; }

        public bool IsValid() 
            => !(string.IsNullOrWhiteSpace(MusicTitle) 
            || string.IsNullOrWhiteSpace(AlbumDirectory));
    }
}