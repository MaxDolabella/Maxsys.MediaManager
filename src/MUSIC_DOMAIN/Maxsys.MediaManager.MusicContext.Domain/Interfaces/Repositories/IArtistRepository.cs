using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.ModelCore.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories
{
    public interface IArtistRepository : IRepositoryBase<Artist>
    {
        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="ArtistInfoDTO"/>.
        /// </summary>
        Task<IReadOnlyList<ArtistInfoDTO>> GetArtistInfosAsync();

        Task<IReadOnlyList<ArtistListDTO>> GetArtistListsAsync();

        Task<int> AlbumsCountAsync(Guid artistId);
    }
}