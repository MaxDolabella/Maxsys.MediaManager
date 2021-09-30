using Maxsys.ModelCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities
{
    [DebuggerDisplay("{Name}")]
    public class Artist : EntityBase
    {
        #region PROPERTIES

        public string Name { get; protected set; }
        public Guid MusicCatalogId { get; protected set; }

        // Navigation
        public virtual MusicCatalog MusicCatalog { get; protected set; }

        // Collections
        public virtual ICollection<Album> Albums { get; protected set; }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        protected Artist()
        {
            Albums = new List<Album>();
        }

        internal Artist(Guid id, string name, Guid musicCatalogId)
            : this()
        {
            (Id, Name, MusicCatalogId) = (id, name, musicCatalogId);
        }

        #endregion CONSTRUCTORS

        public void SetMusicCatalog(MusicCatalog newMusicCatalog)
        {
            MusicCatalog = newMusicCatalog;
            MusicCatalogId = newMusicCatalog?.Id ?? default;
        }
    }
}