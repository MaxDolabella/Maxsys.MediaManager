using System.Diagnostics;
using Maxsys.Core.Entities;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[DebuggerDisplay("{Name}")]
public class Artist : Entity<Guid>
{
    #region PROPERTIES

    public Guid CatalogId { get; protected set; }
    public string Name { get; protected set; }
    public SpotifyID? SpotifyID { get; protected set; }

    // Navigation

    public Catalog Catalog { get; protected set; }

    // Collections
    public List<Album> Albums { get; protected set; } = [];

    #endregion PROPERTIES

    public void SetSpotifyID(string? id)
    {
        if (SpotifyID != id)
        {
            SpotifyID = id;
        }
    }

    #region CONSTRUCTORS

    public Artist()
    { }

    internal Artist(Guid id, Guid catalogId, string name, string? spotifyID)
    {
        Id = id;
        CatalogId = catalogId;

        Name = name;
        SpotifyID = spotifyID;
    }

    #endregion CONSTRUCTORS
}