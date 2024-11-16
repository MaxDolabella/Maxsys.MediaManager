using System.Collections.ObjectModel;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects;

public class PlaylistItemCollection : Collection<PlaylistItem>
{
    private readonly Playlist _owner;

    public PlaylistItemCollection(Playlist playlist) => _owner = playlist;

    protected PlaylistItemCollection()
    { }

    public new void Add(PlaylistItem item)
    {
        if (item is null) return;

        var index = Items.IndexOf(item);

        if (item.SongId == default || index >= 0) return;

        var insertIndex = ~index;
        Items.Insert(insertIndex, item);
    }

    public void Add(Song music, short? order)
    {
        if (music is null) return;

        var item = new PlaylistItem(_owner.Id, music.Id, order);

        Add(item);
    }

    public bool Contains(Song music) => Items.Any(i => i.SongId == music.Id);
}