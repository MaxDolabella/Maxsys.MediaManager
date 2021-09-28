using Maxsys.MediaManager.MusicContext.Domain.Entities;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.Factories
{
    public static class ArtistFactory
    {
        public static Artist Create(Guid id, string name, Guid musicCatalogId)
        {
            return new Artist(id, name, musicCatalogId);
        }
    }
}