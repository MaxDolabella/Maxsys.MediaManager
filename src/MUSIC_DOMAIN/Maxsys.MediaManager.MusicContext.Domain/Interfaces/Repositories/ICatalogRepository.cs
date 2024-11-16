using Maxsys.Core.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.DTO;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

public interface ICatalogRepository : IRepository<Catalog>
{
    /// <summary>
    /// Asynchronously returns a list of <see cref="CatalogDetailDTO"/>.
    /// </summary>
    Task<IReadOnlyList<CatalogDetailDTO>> GetCatalogDetailsAsync(CancellationToken token = default);

    /// <summary>
    /// Asynchronously returns a readonly list of <see cref="CatalogInfoDTO"/>.
    /// </summary>
    Task<IReadOnlyList<CatalogInfoDTO>> GetCatalogInfosAsync(CancellationToken token = default);

    Task<int> ArtistCountAsync(Guid catalogId, CancellationToken token = default);
}