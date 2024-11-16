using AutoMapper;
using Maxsys.Core.Data;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories;

public class ArtistRepository : RepositoryBase<Artist>, IArtistRepository
{
    public ArtistRepository(MusicAppContext context, IMapper mapper) : base(context, mapper)
    { }

    public async Task<IReadOnlyList<ArtistDetailsDTO>> GetArtistDetailsAsync(CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(e => e.Catalog)
            .Select(e => new ArtistDetailsDTO
            {
                ArtistId = e.Id,
                ArtistName = e.Name,
                CatalogId = e.CatalogId,
                CatalogName = e.Catalog.Name
            })
            .OrderBy(m => m.CatalogName)
            .ThenBy(m => m.ArtistName)
            .ToListAsync(token);
    }

    public async Task<IReadOnlyList<ArtistInfoDTO>> GetArtistInfosAsync(CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(e => e.Catalog)
            .Include(e => e.Albums)
            .Select(e => new ArtistInfoDTO
            {
                ArtistId = e.Id,
                ArtistName = e.Name,
                AlbumsCount = e.Albums.Count(),
                CatalogName = e.Catalog.Name
            })
            .OrderBy(d => d.CatalogName)
            .ThenBy(d => d.ArtistName)
            .ToListAsync(token);
    }

    public async Task<int> AlbumsCountAsync(Guid artistId, CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(e => e.Albums)
            .Where(e => e.Id == artistId)
            .Select(e => e.Albums.Count())
            .FirstOrDefaultAsync(token);
    }
}