using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using Maxsys.ModelCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities
{
    [DebuggerDisplay("{Name}[{Items.Count} items]")]
    public class Playlist : EntityBase
    {
        #region PROPERTIES
		public string Name { get; protected set; }

        // Collections
        public virtual PlaylistItemCollection Items { get; protected set; }
        

		#endregion

        #region CONSTRUCTORS
        protected Playlist() => Items = new PlaylistItemCollection(this);

        internal Playlist(Guid id, string name)
        {   
            Id = id;
            Name = name;

            Items = new PlaylistItemCollection(this);
        }
        #endregion

        #region METHODS
        public TimeSpan Duration() 
            => new TimeSpan(Items.Sum(i => i.Music.MusicProperties.Duration.Ticks));

        public double AverageStars10()
            => Items.Average(i => i.Music.Classification.GetStars10());

        public long SizeInBytes()            
            => Items.Sum(i => i.Music.FileSize);
        #endregion
    }
}