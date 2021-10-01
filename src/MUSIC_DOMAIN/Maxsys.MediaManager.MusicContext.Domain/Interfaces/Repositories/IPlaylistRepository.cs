using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.ModelCore.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories
{
    public interface IPlaylistRepository : IRepositoryBase<Playlist>
    {
        //Playlist GetByName(string name);
        //Playlist GetByIdWithDependencies(Guid id);
        bool RemovePlaylistItem(PlaylistItem item);
        bool RemovePlaylistItems(IEnumerable<PlaylistItem> items);
        Task<IReadOnlyList<PlaylistItem>> GetPlaylistItemsByMusicIdAsync(Guid musicId, bool @readonly = false);
    }
}