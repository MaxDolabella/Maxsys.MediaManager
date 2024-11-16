using Maxsys.Core.Entities;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[System.Diagnostics.DebuggerDisplay("{Name}[{Items.Count} items]")]
public class Playlist : Entity<Guid>
{
    #region PROPERTIES

    public string Name { get; protected set; }

    // Collections
    public List<PlaylistItem> Items { get; protected set; } = [];

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Playlist()
    { }

    internal Playlist(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    #endregion CONSTRUCTORS

    #region METHODS

    public TimeSpan Duration()
        => new(Items.Sum(i => i.Song.SongProperties.Duration.Ticks));

    public double AverageStars10()
        => Items.Average(i => i.Song.Classification.GetStars10());

    public long SizeInBytes()
        => Items.Sum(i => i.Song.FileSize);

    #endregion METHODS
}