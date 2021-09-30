using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.ModelCore.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories
{
    public interface IMusicRepository : IRepositoryBase<Music>
    {
        /// <summary>
        /// Asynchronously checks whether a music is already registered, given a music path
        /// </summary>
        /// <param name="musicPath">is the music path to find.</param>
        /// <returns><see langword="true"/> if music exists in music records. Othewise, <see langword="false"/>.</returns>
        Task<bool> PathExistsAsync(string musicPath);

        Task<Music> GetByPathAsync(string musicPath);

        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="MusicInfoDTO"/>.
        /// </summary>
        Task<IReadOnlyList<MusicInfoDTO>> GetMusicsAsync();

        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="MusicListDTO"/>.
        /// </summary>
        Task<IReadOnlyList<MusicListDTO>> GetMusicListAsync();
    }
}