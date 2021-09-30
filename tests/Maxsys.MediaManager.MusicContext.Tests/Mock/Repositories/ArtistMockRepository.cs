using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Tests.Mock.Repositories
{
    internal class ArtistMockRepository : MockRepositoryBase<Artist>, IArtistRepository
    {
        public async Task<IReadOnlyList<ArtistInfoDTO>> GetArtistInfosAsync()
        {
            var items = await GetAllAsync();

            return items.Select(i => new ArtistInfoDTO
            {
                ArtistId = i.Id,
                ArtistName = i.Name,
                MusicCatalogId = i.MusicCatalogId,
                MusicCatalogName = i.MusicCatalog?.Name
            }).ToList();
        }

        public async Task<IReadOnlyList<ArtistListDTO>> GetArtistListsAsync()
        {
            var items = await GetAllAsync();

            return items.Select(i => new ArtistListDTO
            {
                ArtistId = i.Id,
                ArtistName = i.Name,
                MusicCatalogName = i.MusicCatalog?.Name,
                AlbumsCount = i.Albums?.Count ?? -1
            }).ToList();
        }

        public async Task<int> AlbumsCountAsync(Guid id)
        {
            var obj = await GetByIdAsync(id);

            return obj?.Albums?.Count ?? -1;
        }
    }
}