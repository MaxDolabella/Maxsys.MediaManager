using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.ModelCore.Interfaces.Repositories;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

public interface IArtistRepository : IRepositoryBase<Artist>
{
    /// <summary>
    /// Asynchronously returns a list of <see cref="ArtistDetailsDTO"/>.
    /// </summary>
    Task<IReadOnlyList<ArtistDetailsDTO>> GetArtistDetailsAsync(CancellationToken token = default);

    Task<IReadOnlyList<ArtistInfoDTO>> GetArtistInfosAsync(CancellationToken token = default);

    Task<int> AlbumsCountAsync(Guid artistId, CancellationToken token = default);
}