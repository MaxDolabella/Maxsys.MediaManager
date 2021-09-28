namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public struct AlbumTagDTO
    {
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
}