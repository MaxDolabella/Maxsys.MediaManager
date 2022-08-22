namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class AlbumFactory
{
    /// <summary>
    /// Factory for Album
    /// </summary>
    /// <param name="artistId">Required. Id of Album artist.</param>
    /// <param name="albumDirectory">is the artist directory in library (based on <see cref="Artist"/> properties)</param>
    /// <param name="name">Required. Album name.</param>
    /// <param name="year">Optional. Album year</param>
    /// <param name="genre">Required. Album genre.</param>
    /// <param name="albumCover">Album cover picture.</param>
    /// <param name="albumType">Album type.</param>
    public static Album Create(Guid artistId, string albumDirectory, string name, short? year, string genre, byte[] albumCover, AlbumType albumType)
    {
        return new Album(GuidGen.NewSequentialGuid(), artistId, albumDirectory, name, year, genre, albumCover, albumType);
    }
}