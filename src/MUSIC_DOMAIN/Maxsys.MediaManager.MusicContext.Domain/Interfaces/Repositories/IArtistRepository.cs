using Maxsys.Core.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.DTO;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

public interface IArtistRepository : IRepository<Artist>
{
    /// <summary>
    /// Asynchronously returns a list of <see cref="ArtistDetailsDTO"/>.
    /// </summary>
    Task<IReadOnlyList<ArtistDetailsDTO>> GetArtistDetailsAsync(CancellationToken token = default);

    Task<IReadOnlyList<ArtistInfoDTO>> GetArtistInfosAsync(CancellationToken token = default);

    Task<int> AlbumsCountAsync(Guid artistId, CancellationToken token = default);
}