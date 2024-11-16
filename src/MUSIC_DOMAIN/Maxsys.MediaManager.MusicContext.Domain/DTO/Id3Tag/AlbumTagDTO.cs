namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public readonly record struct AlbumTagDTO(string ArtistName, string AlbumName, string AlbumGenre, int? AlbumYear, byte[] AlbumCover)
{
    public bool IsValid()
    {
        return !(string.IsNullOrWhiteSpace(ArtistName)
            || string.IsNullOrWhiteSpace(AlbumName)
            || string.IsNullOrWhiteSpace(AlbumGenre)
            || AlbumCover is null);
    }
}