using System.Diagnostics;
using Maxsys.Core.Entities;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[DebuggerDisplay("{Name}[{Items.Count} items]")]
public class Playlist : Entity<Guid>
{
    #region PROPERTIES

    public string Name { get; protected set; }
    public SpotifyID? SpotifyID { get; protected set; }

    // Navigation
    public List<PlaylistItem> Items { get; protected set; } = [];

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Playlist()
    { }

    internal Playlist(Guid id, string name, string? spotifyID)
    {
        Id = id;
        Name = name;
        SpotifyID = spotifyID;
    }

    #endregion CONSTRUCTORS

    #region METHODS

    public TimeSpan Duration()
        => new(Items.Sum(i => i.Song.SongProperties.Duration.Ticks));

    public double AverageStars10()
        => Items.Average(i => i.Song.Classification.GetStars10());

    public long SizeInBytes()
        => Items.Sum(i => i.Song.FileSize);

    public void SetSpotifyID(string? id)
    {
        if (SpotifyID != id)
        {
            SpotifyID = id;
        }
    }

    #endregion METHODS
}