using Maxsys.MediaManager.MusicContext.Domain.Enums;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public record struct DefineAlbumDirectoryParams(string MusicCatalogName, 
                                                string ArtistName, 
                                                string AlbumName, 
                                                int? AlbumYear, 
                                                AlbumTypes AlbumType)
{

    public bool IsValid() 
        => !string.IsNullOrWhiteSpace(MusicCatalogName)
        && !string.IsNullOrWhiteSpace(ArtistName)
        && !string.IsNullOrWhiteSpace(AlbumName);
    
}