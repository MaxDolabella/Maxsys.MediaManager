namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public struct MusicTagDTO
    {
        public string MusicFullPath { get; init; }
        public string MusicTitle { get; init; }
        public int? MusicTrackNumber { get; init; }
        public byte MusicRating10 { get; init; }
        public string MusicComments { get; init; }
        public string MusicLyrics { get; init; }
        public string MusicCoveredArtist { get; init; }
        public string MusicFeaturedArtist { get; init; }
        public string[] MusicComposers { get; init; }

        private bool IsMusicTrackNumberValid()
        {
            if (MusicTrackNumber is null) return true;

            return MusicTrackNumber.Value >= 0;
        }

        public bool IsValid()
        {
            return !(string.IsNullOrWhiteSpace(MusicFullPath)
                || string.IsNullOrWhiteSpace(MusicTitle)
                || MusicRating10 > 10
                || IsMusicTrackNumberValid()
                || MusicComposers is null);
        }
    }
}