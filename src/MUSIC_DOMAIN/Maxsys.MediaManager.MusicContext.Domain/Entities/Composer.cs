using System;
using System.Collections.Generic;
using System.Diagnostics;
using Maxsys.ModelCore;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities
{
    [DebuggerDisplay("{Name}")]
    public class Composer : EntityBase
    {
        #region PROPERTIES
		public string Name { get; protected set; }


        // Collections
		//public virtual ICollection<MusicComposer> MusicComposer { get; } = new List<MusicComposer>();
        public virtual ICollection<Music> Musics { get; protected set; }
        #endregion

        #region CONSTRUCTORS
        protected Composer() { }

        internal Composer(Guid id, string name)
        {
            Id = id;
            Name = name;

            Musics = new List<Music>();
        }
        #endregion
    }
}