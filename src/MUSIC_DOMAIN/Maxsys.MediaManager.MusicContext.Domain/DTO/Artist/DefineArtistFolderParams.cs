namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public record struct DefineArtistFolderParams(string MusicCatalogName, string ArtistName)
{
    public bool IsValid()
        => !string.IsNullOrWhiteSpace(MusicCatalogName)
        && !string.IsNullOrWhiteSpace(ArtistName);
}