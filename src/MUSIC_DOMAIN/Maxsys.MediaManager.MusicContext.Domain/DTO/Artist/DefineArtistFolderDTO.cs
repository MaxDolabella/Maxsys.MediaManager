namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public struct DefineArtistFolderDTO
{
    public string MusicCatalogName { get; init; }
    public string ArtistName { get; init; }

    public bool IsValid()
        => !(string.IsNullOrWhiteSpace(MusicCatalogName)
        || string.IsNullOrWhiteSpace(ArtistName));
}