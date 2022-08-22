namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct AlbumTagDTO
{
    public AlbumTagDTO(string artistName, string albumName, string albumGenre, int? albumYear, byte[] albumCover)
    {
        ArtistName = artistName;
        AlbumName = albumName;
        AlbumGenre = albumGenre;
        AlbumYear = albumYear;
        AlbumCover = albumCover;
    }

    public string ArtistName { get; init; }
    public string AlbumName { get; init; }
    public string AlbumGenre { get; init; }
    public int? AlbumYear { get; init; }
    public byte[] AlbumCover { get; init; }

    public bool IsValid()
    {
        return !(string.IsNullOrWhiteSpace(ArtistName)
            || string.IsNullOrWhiteSpace(AlbumName)
            || string.IsNullOrWhiteSpace(AlbumGenre)
            || AlbumCover is null);
    }
}