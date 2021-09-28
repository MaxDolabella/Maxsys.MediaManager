using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories
{
    /// <inheritdoc cref="IMusicCatalogRepository" />
    public class MusicCatalogRepository : RepositoryBase<MusicCatalog>, IMusicCatalogRepository
    {
        public MusicCatalogRepository(MusicAppContext dbContext) : base(dbContext)
        { }

        public async Task<IReadOnlyList<MusicCatalogInfoDTO>> GetMusicCatalogListAsync()
        {
            return await ReadOnlySet
                .Select(entity => new MusicCatalogInfoDTO
                {
                    MusicCatalogId = entity.Id,
                    MusicCatalogName = entity.Name
                }).ToListAsync();
        }

        public async Task<IReadOnlyList<MusicCatalogListDTO>> GetMusicCatalogListsAsync()
        {
            return await ReadOnlySet
                .Include(e => e.Artists)
                .Select(e => new MusicCatalogListDTO
                {
                    MusicCatalogId = e.Id,
                    MusicCatalogName = e.Name,
                    ArtistCount = e.Artists.Count
                })
                .OrderBy(m => m.MusicCatalogName)
                .ToListAsync();
        }

        public async Task<int> ArtistsCountAsync(Guid id)
        {
            return await ReadOnlySet
                .Include(e => e.Artists)
                .Where(e => e.Id == id)
                .Select(e => e.Artists.Count)
                .FirstOrDefaultAsync();
        }
    }
}