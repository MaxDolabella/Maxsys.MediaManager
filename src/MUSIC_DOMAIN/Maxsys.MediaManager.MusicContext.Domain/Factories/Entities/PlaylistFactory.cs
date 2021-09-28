using Maxsys.MediaManager.MusicContext.Domain.Entities;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.Factories
{
    public static class PlaylistFactory
    {
        public static Playlist Create(Guid id, string name)
        {
            return new Playlist(id, name);
        }
    }
}