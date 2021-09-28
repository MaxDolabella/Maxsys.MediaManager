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
        #endregion

        #region CONSTRUCTORS
        protected Artist() 
        {
            Albums = new List<Album>();
        }

        [Obsolete("Mvvm version really needs this ctor?")]
        internal Artist(Guid id, string name, MusicCatalog musicCatalog)
        {
            (Id, Name, MusicCatalog) = (id, name, musicCatalog);

            if (musicCatalog != null) MusicCatalogId = musicCatalog.Id;
        }

        internal Artist(Guid id, string name, Guid musicCatalogId)
        {
            (Id, Name, MusicCatalogId) = (id, name, musicCatalogId);
        }
        #endregion
    }
}