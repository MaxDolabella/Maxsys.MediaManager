using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Tests.Mock.Repositories
{
    internal class MusicCatalogMockRepository : MockRepositoryBase<MusicCatalog>, IMusicCatalogRepository
    {
        public async Task<IReadOnlyList<MusicCatalogInfoDTO>> GetMusicCatalogListAsync()
        {
            var items = await GetAllAsync();

            return items.Select(i => new MusicCatalogInfoDTO
            {
                MusicCatalogId = i.Id,
                MusicCatalogName = i.Name
            }).ToList();
        }

        public async Task<IReadOnlyList<MusicCatalogListDTO>> GetMusicCatalogListsAsync()
        {
            var items = await GetAllAsync();

            return items.Select(i => new MusicCatalogListDTO
            {
                MusicCatalogId = i.Id,
                MusicCatalogName = i.Name,
                ArtistCount = i.Artists?.Count ?? -1
            }).ToList();
        }

        public async Task<int> ArtistsCountAsync(Guid id)
        {
            var obj = await GetByIdAsync(id);

            return obj?.Artists?.Count ?? -1;
        }
    }
}