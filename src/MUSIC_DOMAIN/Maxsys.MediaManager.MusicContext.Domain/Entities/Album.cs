using System;
using System.Collections.Generic;
using System.Diagnostics;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using Maxsys.ModelCore;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities
{
    [DebuggerDisplay("[{Id.ToString().Substring(0,4)}] {Name}")]
    public class Album : EntityBase
    {
        #region PROPERTIES
        /// <summary>
        /// Path limit is 248 characters.
        /// Path+Filename limit is 260 characters.
        /// </summary>
        public string AlbumDirectory { get; protected set; }
        public string Name { get; protected set; }
        public int? Year { get; protected set; }
        public string Genre { get; protected set; }
        public AlbumType AlbumType { get; protected set; }
        public byte[] AlbumCover { get; protected set; }


        // Navigation
        public virtual Guid ArtistId { get; protected set; }
        public virtual Artist Artist { get; protected set; }

        // Collections
		public virtual ICollection<Music> Musics { get; protected set; }
		#endregion

        public string GetYearName()
        {
            var yearString = Year.HasValue ? $"({Year.Value}) " : "";

            return yearString + Name;
        }

        #region CONSTRUCTORS
        protected Album() { Musics = new List<Music>(); }

        [Obsolete]
        internal Album(Guid id, string albumDirectory, string name, int? year, string genre, byte[] albumCover, AlbumType albumType, Artist artist)
        {
            (Id, AlbumDirectory, Name, Year, Genre, AlbumCover, AlbumType, Artist) =
            (id, albumDirectory, name, year, genre, albumCover, albumType, artist);

            if (artist != null) ArtistId = artist.Id;
        }

        internal Album(Guid id, string albumDirectory, string name, int? year, string genre, byte[] albumCover, AlbumType albumType, Guid artistId)
        {
            (Id, AlbumDirectory, Name, Year, Genre, AlbumCover, AlbumType, ArtistId) =
            (id, albumDirectory, name, year, genre, albumCover, albumType, artistId);
        }

        #endregion
    }
}