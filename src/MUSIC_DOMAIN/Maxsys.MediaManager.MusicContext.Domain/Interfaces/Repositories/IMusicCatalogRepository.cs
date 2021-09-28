using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.ModelCore.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories
{
    public interface IMusicCatalogRepository : IRepositoryBase<MusicCatalog>
    {
        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="MusicCatalogInfoDTO"/>.
        /// </summary>
        Task<IReadOnlyList<MusicCatalogInfoDTO>> GetMusicCatalogListAsync();
        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="MusicCatalogListDTO"/>.
        /// </summary>
        Task<IReadOnlyList<MusicCatalogListDTO>> GetMusicCatalogListsAsync();
        
        Task<int> ArtistsCountAsync(Guid id);
    }
}