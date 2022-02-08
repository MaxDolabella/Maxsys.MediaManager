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
    public class PlaylistRepository : RepositoryBase<Playlist>, IPlaylistRepository
    {
        public PlaylistRepository(MusicAppContext dbContext) : base(dbContext)
        { }

        public async Task<IReadOnlyList<PlaylistItem>> GetPlaylistItemsByMusicIdAsync(Guid musicId, bool @readonly = false)
        {
            return await DbSet.AsNoTracking(!@readonly)
                .SelectMany(p => p.Items)
                .Where(i => i.MusicId == musicId)
                .ToListAsync();
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

            var result = items.All(i => Context.Entry(i).State is EntityState.Detached or EntityState.Deleted);

            return result;
        }

        public async Task<IEnumerable<Playlist>> GetAllWithDependenciesAsync()
        {
            return await DbSet.AsNoTracking(track: false)
                .Include(p => p.Items)
                    .ThenInclude(i => i.Music)
                .ToListAsync();
        }
    }
}