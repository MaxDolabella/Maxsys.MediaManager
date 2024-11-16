using AutoMapper;
using Maxsys.Core.Data;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories;

public class PlaylistRepository : RepositoryBase<Playlist>, IPlaylistRepository
{
    public PlaylistRepository(MusicAppContext context, IMapper mapper) : base(context, mapper)
    { }

    public async Task<IReadOnlyList<PlaylistItem>> GetPlaylistItemsBySongIdAsync(Guid songId, bool @readonly = false, CancellationToken token = default)
    {
        return await DbSet.SelectMany(p => p.Items)
            .Where(i => i.SongId == songId)
            .ToListAsync(token);
    }

    public async Task<IReadOnlyList<PlaylistDTO>> GetPlaylistBySongIdAsync(Guid songId, CancellationToken token = default)
    {
        var query1 = DbSet.AsNoTracking()
            .SelectMany(p => p.Items)
            .Where(i => i.SongId == songId)
            .Select(i => new PlaylistDTO(i.PlaylistId, i.Playlist.Name))
            .DistinctBy(p => p.PlaylistId);

        var query2 = DbSet.AsNoTracking()
            .Where(p => p.Items.Any(i => i.SongId == songId))
            .Select(p => new PlaylistDTO(p.Id, p.Name))
            .DistinctBy(p => p.PlaylistId);

        // TODO Analisar queries

        return await query2.ToListAsync(token);
    }

    public bool RemovePlaylistItem(PlaylistItem item)
    {
        var tracker = Context.Set<PlaylistItem>().Remove(item);

        return tracker.State
            is EntityState.Detached
            or EntityState.Deleted;
    }

    public bool RemovePlaylistItems(IEnumerable<PlaylistItem> items)
    {
        Context.Set<PlaylistItem>().RemoveRange(items);

        return items.All(i => Context.Entry(i).State
            is EntityState.Detached
            or EntityState.Deleted);
    }

    //public async Task<IEnumerable<Playlist>> GetAllWithDependenciesAsync(CancellationToken token = default)
    //{
    //    return await DbSet
    //        .Include(p => p.Items)
    //            .ThenInclude(i => i.Song)
    //        .ToListAsync(token);
    //}
}