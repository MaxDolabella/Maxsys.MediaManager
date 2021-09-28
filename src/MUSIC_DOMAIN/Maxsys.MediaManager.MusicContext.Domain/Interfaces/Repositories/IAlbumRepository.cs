using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.ModelCore.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories
{
    public interface IAlbumRepository : IRepositoryBase<Album>
    {
        /// <summary>
        /// Asynchronouly retrives a readonly list of all genres in repository.
        /// </summary>
        Task<IReadOnlyList<string>> GetGenresAsync();

        /// <summary>
        /// Asynchronouly retrives an <see cref="AlbumTagDTO"/> dto with album/artist info for id3v2 tag.
        /// </summary>
        /// <param name="albumId"></param>
        Task<AlbumTagDTO> GetAlbumTagDTO(Guid albumId);

        /// <summary>
        /// Gets a list of TrackNumber and Title from a specific album asynchronous
        /// </summary>
        /// <param name="albumId"></param>
        Task<IReadOnlyList<MusicTrackAndTitleDTO>> GetTracksAndTitlesAsync(Guid albumId);

        /// <summary>
        /// Returns a readonly list of <see cref="AlbumInfoDTO"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<AlbumInfoDTO>> GetAlbumInfosAsync();
        Task<IReadOnlyList<AlbumListDTO>> GetAlbumListsAsync();

        /// <summary>
        /// Asynchronously return the musics count in the album
        /// </summary>
        /// <param name="albumId">The id of the album to count the musics.</param>
        /// <returns>The musics count.</returns>
        Task<int> MusicsCountAsync(Guid albumId);
    }
}