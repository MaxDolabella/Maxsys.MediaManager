using Maxsys.ModelCore;
using System;
using System.Diagnostics;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities
{
    [DebuggerDisplay("Order={Order}|[{Music.Classification.Rating}] {Music.Title}")]
    public class PlaylistItem : /*EntityBase,*/ IComparable, IComparable<PlaylistItem>
    {
        #region PROPERTIES
        public uint Order { get; protected set; }

        // Navigation
        public virtual Playlist Playlist { get; protected set; }
        public Guid PlaylistId { get; protected set; }
        public virtual Music Music { get; protected set; }
		public Guid MusicId { get; protected set; }
		#endregion


        #region CONSTRUCTORS
        protected PlaylistItem() { }

        
        internal PlaylistItem(Playlist playlist, Music music, uint order) 
            //: this(id, music.Id, order)
        {
            (Playlist, PlaylistId, Music, MusicId, Order)
                = (playlist, playlist.Id, music, music.Id, order);
        }
        #endregion


        #region IComparable, IComparable<PlaylistItem>
        public int CompareTo(object obj)
        {
            return obj is not PlaylistItem playlistItem || playlistItem is null
                ? 1
                : CompareTo(playlistItem);
        }

        public int CompareTo(PlaylistItem other)
        {
            if (other is null) return 1;

            var order1 = this.Order == 0 ? uint.MaxValue : this.Order;
            var order2 = other.Order == 0 ? uint.MaxValue : other.Order;
            
            var orderComparer = order1.CompareTo(order2);
            if (orderComparer != 0)
                return orderComparer;

            var ratingComparer = -(this.Music.Classification.Rating.CompareTo(other.Music?.Classification.Rating));
            if (ratingComparer != 0)
                return ratingComparer;

            var title1 = $"{this.Music.TrackNumber} {this.Music.Title}";
            var title2 = $"{other.Music.TrackNumber} {other.Music.Title}";
            var titleComparer = title1.CompareTo(title2);
            return titleComparer != 0 
                ? titleComparer 
                : this.Music.Id.CompareTo(other.Music.Id);
        }
        #endregion
    }
}