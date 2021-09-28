using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories
{
    public class PlaylistRepository : RepositoryBase<Playlist>, IPlaylistRepository
    {
        public PlaylistRepository(MusicAppContext dbContext) : base(dbContext) { }


        public Playlist GetByName(string name)
        {
            return DbSet.Where(x => x.Name == name).FirstOrDefault();
        }

        public Playlist GetByIdWithDependencies(Guid id)
        {
            return DbSet
                .Include(pl => pl.Items)
                    .ThenInclude(i => i.Music)
                    .ThenInclude(m => m.Album)
                .FirstOrDefault(pl => pl.Id == id);
        }
    }
}
