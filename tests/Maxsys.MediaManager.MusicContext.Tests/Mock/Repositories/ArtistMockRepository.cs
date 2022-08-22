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
        public async Task<IReadOnlyList<ArtistDetailsDTO>> GetArtistInfosAsync()
        {
            var items = await GetAllAsync();

            return items.Select(i => new ArtistDetailsDTO
            {
                ArtistId = i.Id,
                ArtistName = i.Name,
                MusicCatalogId = i.CatalogId,
                MusicCatalogName = i.Catalog?.Name
            }).ToList();
        }

        public async Task<IReadOnlyList<ArtistInfoDTO>> GetArtistListsAsync()
        {
            var items = await GetAllAsync();

            return items.Select(i => new ArtistInfoDTO
            {
                ArtistId = i.Id,
                ArtistName = i.Name,
                MusicCatalogName = i.Catalog?.Name,
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