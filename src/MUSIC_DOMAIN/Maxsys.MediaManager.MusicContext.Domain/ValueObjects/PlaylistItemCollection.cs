using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Factories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects
{
    public class PlaylistItemCollection : ICollection<PlaylistItem>
    {
        readonly List<PlaylistItem> _items = new List<PlaylistItem>();
        readonly Playlist _owner;
        public PlaylistItemCollection(Playlist playlist) => _owner = playlist;
        protected PlaylistItemCollection() { }

        public void Add(PlaylistItem item)
        {
            if (item is null) return;

            var index = _items.BinarySearch(item);
            
            if (item.MusicId == default || index >= 0) return;

            var insertIndex = ~index;
            _items.Insert(insertIndex, item);
        }

        public void Add(Music music, uint order)
        {
            if (music is null) return;

            var item = new PlaylistItem(_owner, music, order);

            Add(item);
        }

        public void Clear() => _items.Clear();

        public bool Contains(PlaylistItem item) => _items.Contains(item);
        public bool Contains(Music music) => _items.Any(i => i.MusicId == music.Id);

        public void CopyTo(PlaylistItem[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count) throw new ArgumentException("Not enough elements after arrayIndex in the destination array.");

            for (int i = 0; i < Count; ++i)
                array[i + arrayIndex] = this[i];
        }

        public bool Remove(PlaylistItem item) => _items.Remove(item);

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public IEnumerator<PlaylistItem> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public PlaylistItem this[int index] => _items[index];

        public void Sort() => _items.Sort();
    }
}
