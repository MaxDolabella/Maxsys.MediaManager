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
    public class ArtistRepository : RepositoryBase<Artist>, IArtistRepository
    {
        public ArtistRepository(MusicAppContext dbContext) : base(dbContext)
        { }

        public async Task<IReadOnlyList<ArtistInfoDTO>> GetArtistInfosAsync()
        {
            return await ReadOnlySet
                .Include(e => e.MusicCatalog)
                .Select(e => new ArtistInfoDTO
                {
                    ArtistId = e.Id,
                    ArtistName = e.Name,
                    MusicCatalogId = e.MusicCatalogId,
                    MusicCatalogName = e.MusicCatalog.Name
                })
                .OrderBy(m => m.MusicCatalogName)
                .ThenBy(m => m.ArtistName)
                .ToListAsync();
        }
        public async Task<IReadOnlyList<ArtistListDTO>> GetArtistListsAsync()
        {
            return await ReadOnlySet
                .Include(e => e.MusicCatalog)
                .Include(e => e.Albums)
                .Select(e => new ArtistListDTO
                {
                    ArtistId = e.Id,
                    ArtistName = e.Name,
                    AlbumsCount = e.Albums.Count,
                    MusicCatalogName = e.MusicCatalog.Name
                })
                .OrderBy(d => d.MusicCatalogName)
                .ThenBy(d => d.ArtistName)
                .ToListAsync();
        }
        public async Task<int> AlbumsCountAsync(Guid artistId)
        {
            return await ReadOnlySet
                .Include(e => e.Albums)
                .Where(e => e.Id == artistId)
                .Select(e => e.Albums.Count)
                .FirstOrDefaultAsync();
        }
    }
}