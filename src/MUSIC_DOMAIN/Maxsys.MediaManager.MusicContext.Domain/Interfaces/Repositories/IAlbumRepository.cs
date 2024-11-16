using Maxsys.Core.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.DTO;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

public interface IAlbumRepository : IRepository<Album>
{
    /// <summary>
    /// Asynchronouly retrives a list of all genres in repository.
    /// </summary>
    Task<IReadOnlyList<string>> GetGenresAsync(CancellationToken token = default);

    /// <summary>
    /// Asynchronouly retrives an <see cref="AlbumTagDTO"/> with album/artist info for id3v2 tag.
    /// </summary>
    /// <param name="albumId"></param>
    Task<AlbumTagDTO?> GetAlbumTagAsync(Guid albumId, CancellationToken token = default);

    /// <summary>
    /// Gets a list of TrackNumber and Title from a specific album asynchronous
    /// </summary>
    /// <param name="albumId"></param>
    Task<IReadOnlyList<TrackAndTitleDTO>> GetTrackAndTitlesAsync(Guid albumId, CancellationToken token = default);

    /// <summary>
    /// Returns a list of <see cref="AlbumInfoDTO"/>.
    /// </summary>
    Task<IReadOnlyList<AlbumInfoDTO>> GetAlbumInfosAsync(CancellationToken token = default);

    /// <summary>
    /// Returns a list of <see cref="AlbumDetailDTO"/>.
    /// </summary>
    Task<IReadOnlyList<AlbumDetailDTO>> GetAlbumDetailsAsync(CancellationToken token = default);

    /// <summary>
    /// Asynchronously return the musics count in the album
    /// </summary>
    /// <param name="albumId">The id of the album to count the musics.</param>
    /// <returns>The musics count.</returns>
    Task<int> SongsCountAsync(Guid albumId, CancellationToken token = default);
}