using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.ModelCore.Interfaces.Repositories;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories
{
    public interface IPlaylistRepository : IRepositoryBase<Playlist>
    {
        Playlist GetByName(string name);
        Playlist GetByIdWithDependencies(Guid id);
    }
}