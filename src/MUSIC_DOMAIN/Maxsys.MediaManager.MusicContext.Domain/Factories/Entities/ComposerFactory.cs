using Maxsys.MediaManager.MusicContext.Domain.Entities;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.Factories
{
    public static class ComposerFactory
    {
        public static Composer Create(Guid id, string name)
        {
            return new Composer(id, name);
        }
    }
}