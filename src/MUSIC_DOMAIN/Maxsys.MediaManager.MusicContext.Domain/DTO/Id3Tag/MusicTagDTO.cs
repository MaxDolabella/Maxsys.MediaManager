namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public struct MusicTagDTO
{
    public string MusicFullPath { get; init; }
    public string SongTitle { get; init; }
    public int? SongTrackNumber { get; init; }
    public byte MusicRating10 { get; init; }
    public string MusicComments { get; init; }
    public string MusicLyrics { get; init; }
    public string SongCoveredArtist { get; init; }
    public string SongFeaturedArtist { get; init; }
    public string[] MusicComposers { get; init; }

    private bool IsMusicTrackNumberValid()
    {
        if (SongTrackNumber is null) return true;

        return SongTrackNumber.Value >= 0;
    }

    public bool IsValid()
    {
        return !(string.IsNullOrWhiteSpace(MusicFullPath)
            || string.IsNullOrWhiteSpace(SongTitle)
            || MusicRating10 > 10
            || IsMusicTrackNumberValid()
            || MusicComposers is null);
    }
}