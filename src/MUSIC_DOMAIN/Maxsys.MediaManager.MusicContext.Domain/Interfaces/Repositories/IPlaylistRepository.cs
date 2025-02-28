using Maxsys.Core.Interfaces.Repositories;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

public interface IPlaylistRepository : IRepository<Playlist>
{
    bool RemovePlaylistItem(PlaylistItem item);

    bool RemovePlaylistItems(IEnumerable<PlaylistItem> items);

    Task<IReadOnlyList<PlaylistItem>> GetPlaylistItemsBySongIdAsync(Guid songId, bool @readonly = false, CancellationToken cancellationToken = default);

    Task<Playlist?> GetToExportAsync(Guid id, CancellationToken cancellationToken = default);
    //Task<IEnumerable<Playlist>> GetAllWithDependenciesAsync(CancellationToken token = default);
}