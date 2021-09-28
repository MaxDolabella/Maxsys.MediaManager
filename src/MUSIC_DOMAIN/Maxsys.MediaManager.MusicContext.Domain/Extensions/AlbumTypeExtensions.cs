namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects
{
    public static class AlbumTypeExtensions
    {
        public static string AlbumTypeToString(this AlbumType albumType)
        {
            return albumType switch
            {
                AlbumType.Undefined or AlbumType.Various => string.Empty,
                AlbumType.Studio => "Studio",
                AlbumType.Live => "Live",
                AlbumType.Compilation => "Compilation",
                AlbumType.Bootleg => "Bootleg",
                _ => "Others",
            };
        }
    }
}