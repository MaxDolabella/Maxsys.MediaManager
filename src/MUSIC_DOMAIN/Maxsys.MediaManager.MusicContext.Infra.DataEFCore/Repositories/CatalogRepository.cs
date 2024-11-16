using AutoMapper;
using Maxsys.Core.Data;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories;

/// <inheritdoc cref="ICatalogRepository" />
public class CatalogRepository : RepositoryBase<Catalog>, ICatalogRepository
{
    public CatalogRepository(MusicAppContext context, IMapper mapper) : base(context, mapper)
    { }

    public async Task<IReadOnlyList<CatalogDetailDTO>> GetCatalogDetailsAsync(CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Select(entity => new CatalogDetailDTO(entity.Id, entity.Name))
            .ToListAsync(token);
    }

    public async Task<IReadOnlyList<CatalogInfoDTO>> GetCatalogInfosAsync(CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(e => e.Artists)
            .Select(e => new CatalogInfoDTO(e.Id, e.Name, e.Artists.Count))
            .OrderBy(m => m.MusicCatalogName)
            .ToListAsync(token);
    }

    public async Task<int> ArtistCountAsync(Guid catalogId, CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(e => e.Artists)
            .Where(e => e.Id == catalogId)
            .Select(e => e.Artists.Count)
            .FirstOrDefaultAsync(token);
    }
}