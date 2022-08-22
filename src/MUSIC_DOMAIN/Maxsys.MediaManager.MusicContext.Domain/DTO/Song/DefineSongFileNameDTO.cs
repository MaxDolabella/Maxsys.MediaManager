namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public struct DefineSongFileNameDTO
{
    public int? SongTrackNumber { get; init; }
    public string SongTitle { get; init; }
    public string SongCoveredArtist { get; init; }
    public string SongFeaturedArtist { get; init; }
    public bool SongIsBonusTrack { get; init; }
    public string AlbumDirectory { get; set; }

    public bool IsValid() 
        => !(string.IsNullOrWhiteSpace(SongTitle) 
        || string.IsNullOrWhiteSpace(AlbumDirectory));
}