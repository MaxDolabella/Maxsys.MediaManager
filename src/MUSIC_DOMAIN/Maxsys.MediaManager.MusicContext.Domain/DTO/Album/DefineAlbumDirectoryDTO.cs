using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public struct DefineAlbumDirectoryDTO
    {
        public string MusicCatalogName { get; init; }
        public string ArtistName { get; init; }
        public string AlbumName { get; init; }
        public int? AlbumYear { get; init; }
        public AlbumType AlbumType { get; init; }

        public bool IsValid()
        {
            return !(string.IsNullOrWhiteSpace(MusicCatalogName)
                || string.IsNullOrWhiteSpace(ArtistName)
                || string.IsNullOrWhiteSpace(AlbumName));
        }
    }
}