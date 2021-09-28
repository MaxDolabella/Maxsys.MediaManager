using Maxsys.MediaManager.MusicContext.Domain.Entities;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.Factories
{
    public static class MusicCatalogFactory
    {
        public static MusicCatalog Create(Guid id, string name)
        {
            return new MusicCatalog(id, name);
        }
    }
}