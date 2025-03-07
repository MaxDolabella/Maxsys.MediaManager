using Maxsys.MediaManager.MusicContext.Domain.Enums;

namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class AlbumFactory
{
    /// <summary>
    /// Factory for Album
    /// </summary>
    /// <param name="artistId">Required. Id of Album artist.</param>
    /// <param name="directory">is the artist directory in library (based on <see cref="Artist"/> properties)</param>
    /// <param name="name">Required. Album name.</param>
    /// <param name="year">Optional. Album year</param>
    /// <param name="genre">Required. Album genre.</param>
    /// <param name="cover">Album cover picture.</param>
    /// <param name="type">Album type.</param>
    public static Album Create(Guid id, Guid artistId, string directory, string name, short? year, string genre, byte[] cover, AlbumTypes type, string? spotifyID)
        => new(id, artistId, new(directory), name, year, genre, cover, type, spotifyID);

    public static Album Create(Guid artistId, string directory, string name, short? year, string genre, byte[] albumCover, AlbumTypes albumType, string? spotifyID)
        => Create(GuidGen.NewSequentialGuid(), artistId, new(directory), name, year, genre, albumCover, albumType, spotifyID);
}