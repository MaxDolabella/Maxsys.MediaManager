using System.Diagnostics;
using Maxsys.Core.Entities;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[DebuggerDisplay("{Name}[{Items.Count} items]")]
public class Playlist : Entity<Guid>
{
    #region PROPERTIES

    public string Name { get; protected set; }
    public string? SpotifyRef { get; protected set; }

    // Navigation
    public List<PlaylistItem> Items { get; protected set; } = [];

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Playlist()
    { }

    internal Playlist(Guid id, string name, string? spotifyRef)
    {
        Id = id;
        Name = name;
        SpotifyRef = spotifyRef;
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