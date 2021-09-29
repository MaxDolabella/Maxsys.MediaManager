using Maxsys.ModelCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities
{
    [DebuggerDisplay("{Name}")]
    public class MusicCatalog : EntityBase
    {
        #region PROPERTIES

        public string Name { get; protected set; }

        // Collections
        public virtual ICollection<Artist> Artists { get; protected set; }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        protected MusicCatalog()
        {
            Artists = new List<Artist>();
        }

        internal MusicCatalog(Guid id, string name) : this()
        {
            Id = id;
            Name = name;
        }

        #endregion CONSTRUCTORS
    }
}