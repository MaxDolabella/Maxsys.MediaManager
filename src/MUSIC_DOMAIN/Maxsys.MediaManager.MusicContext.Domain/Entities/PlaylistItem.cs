using System.Diagnostics;
using Maxsys.Core.Entities;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[DebuggerDisplay("Order={Order}|[{Id}")]
public class PlaylistItem : Entity, IComparable, IComparable<PlaylistItem>
{
    // ID = { PlaylistId, Id }

    #region PROPERTIES

    public Guid PlaylistId { get; protected set; }
    public Guid SongId { get; protected set; }

    public short? Order { get; protected set; }

    // Navigation

    public Playlist Playlist { get; protected set; }
    public Song Song { get; protected set; }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected PlaylistItem()
    { }

    internal PlaylistItem(Guid playlistId, Guid songId, short? order)
    {
        (PlaylistId, SongId, Order) = (playlistId, songId, order);
    }

    #endregion CONSTRUCTORS

    #region IComparable, IComparable<PlaylistItem>

    public int CompareTo(object? obj)
    {
        return obj is PlaylistItem playlistItem
            ? CompareTo(playlistItem)
            : 1;
    }

    public int CompareTo(PlaylistItem? other)
    {
        if (other is null) return 1;

        short order1 = Order ?? short.MaxValue;
        short order2 = other.Order ?? short.MaxValue;

        var orderComparer = order1.CompareTo(order2);
        if (orderComparer != 0)
            return orderComparer;

        var ratingComparer = -(Song.Classification.Rating.CompareTo(other.Song?.Classification.Rating));
        if (ratingComparer != 0)
            return ratingComparer;

        var title1 = $"{Song.TrackNumber} {Song.Title}";
        var title2 = $"{other.Song!.TrackNumber} {other.Song.Title}";
        var titleComparer = title1.CompareTo(title2);

        return titleComparer != 0
            ? titleComparer
            : this.Song.Id.CompareTo(other.Song.Id);
    }

    public override bool Equals(object? obj)
    {
        return (obj is PlaylistItem playlistItem)
            && Equals(playlistItem);
    }

    public bool Equals(PlaylistItem? other)
    {
        return ReferenceEquals(this, other)
            || (other is not null && (PlaylistId, SongId) == (other.PlaylistId, other.SongId));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PlaylistId, SongId);
    }

    public static bool operator ==(PlaylistItem? left, PlaylistItem? right)
    {
        return left is null ? right is null : left.Equals(right);
    }

    public static bool operator !=(PlaylistItem? left, PlaylistItem? right)
    {
        return !(left == right);
    }

    public static bool operator <(PlaylistItem? left, PlaylistItem? right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    public static bool operator <=(PlaylistItem? left, PlaylistItem? right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    public static bool operator >(PlaylistItem? left, PlaylistItem? right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    public static bool operator >=(PlaylistItem? left, PlaylistItem? right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }

    #endregion IComparable, IComparable<PlaylistItem>
}